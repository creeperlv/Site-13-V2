using Site13Kernel.Core;
using Site13Kernel.Diagnostics;
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
                    SceneManager.LoadScene(TargetSceneID);
                }
            }
        }
    }
    public class UEFIBase : ControlledBehavior
    {
        public new virtual void Init()
        {
           

        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task Run()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

        }
    }
}
