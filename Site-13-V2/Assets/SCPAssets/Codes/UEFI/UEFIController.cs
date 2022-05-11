using Site13Kernel.Core;
using Site13Kernel.Diagnostics;
using Site13Kernel.GameLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Site13Kernel.UEFI
{
    public class UEFIController : MonoBehaviour
    {
        public List<UEFIBase> EFIs=new List<UEFIBase>();
        public List<ParameterProcessor> PreBoot_ParameterProcessors=new List<ParameterProcessor>();
        public List<ParameterProcessor> PostBoot_ParameterProcessors=new List<ParameterProcessor>();
        public float SplashScreenLength=10f;
        public int TargetSceneID;
        public int UEFISceneID;
        public int SetupUtilitySceneID;
        bool isCompleted;
        void Start()
        {
            foreach (var item in EFIs)
            {
                item.Init();
            }
            var args=Environment.GetCommandLineArgs();
            foreach (var item in PreBoot_ParameterProcessors)
            {
                __INTERRUPT |= item.Process(args);
            }
            Task.Run(async () =>
            {

                foreach (var item in EFIs)
                {
                    try
                    {
                        await item.Run();
                    }
                    catch (System.Exception e)
                    {
                        Debugger.CurrentDebugger.Log(e.Message, LogLevel.Error);
                    }
                }
                isCompleted = true;
            });

        }
        bool __INTERRUPT = false;
        bool __post_boot = false;
        float TimeD=0;
        void Update()
        {
            TimeD += Time.deltaTime;
            if (Input.GetKey(KeyCode.Delete))
            {
                SceneLoader.Instance.AddSceneLog(UEFISceneID, false, false);
                SceneLoader.Instance.LoadScene(SetupUtilitySceneID, true, false, false);
                __INTERRUPT = true;
            }
            if (TimeD > SplashScreenLength)
            {
                if (isCompleted)
                {
                    if (__post_boot == false)
                    {
                        var args = Environment.GetCommandLineArgs();
                        foreach (var item in PostBoot_ParameterProcessors)
                        {
                            __INTERRUPT |= item.Process(args);
                        }
                        __post_boot = true;
                    }
                    if (__INTERRUPT) return;
                    SceneLoader.Instance.AddSceneLog(UEFISceneID, false, false);
                    SceneLoader.Instance.LoadScene(TargetSceneID,true,false,false);

                    isCompleted = false;
                    //SceneManager.UnloadSceneAsync(0);
                }
            }
        }
    }
    public class UEFIBase : ControlledBehavior
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task Run()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

        }
    }
}
