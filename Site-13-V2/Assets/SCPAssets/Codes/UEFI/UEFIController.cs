using Site13Kernel.Core;
using Site13Kernel.Diagnostics;
using Site13Kernel.GameLogic;
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
        public float SplashScreenLength=10f;
        public int TargetSceneID;
        public int UEFISceneID;
        bool isCompleted;
        void Start()
        {
            foreach (var item in EFIs)
            {
                item.Init();
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
        float TimeD=0;
        void Update()
        {
            TimeD += Time.deltaTime;
            if (TimeD > SplashScreenLength)
            {
                if (isCompleted)
                {
                    SceneLoader.Instance.AddSceneLog(UEFISceneID, false, false);
                    SceneLoader.Instance.LoadScene(TargetSceneID,true,false,false);
                    isCompleted = false;
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
