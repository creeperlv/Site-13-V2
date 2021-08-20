using Site13Kernel.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class BehaviorController : BaseController
    {
        public bool CrossScene=false;
        void Start()
        {
            if (CrossScene)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            SerializeAll();
            foreach (var item in _OnInit)
            {
                try
                {
                    InitBehavior(item);
                }
                catch (System.Exception e)
                {
                    Debugger.CurrentDebugger.Log(e, LogLevel.Error);
                }
            }
            foreach (var item in _OnInit)
            {
                try
                {
                    item.Init();

                }
                catch (System.Exception e)
                {
                    Debugger.CurrentDebugger.Log(e, LogLevel.Error);
                }
            }
        }
        void Update()
        {
            float DeltaTime=Time.deltaTime;
            foreach (var item in _OnRefresh)
            {
#if DEBUG
                try
                {
#endif
                    item.Refresh(DeltaTime);
#if DEBUG
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                    Debugger.CurrentDebugger.Log(e, LogLevel.Error);
                }
#endif
            }
        }
        private void FixedUpdate()
        {

            float DeltaTime=Time.fixedDeltaTime;
            foreach (var item in _OnFixedRefresh)
            {
#if DEBUG
                try
                {
#endif
                    item.FixedRefresh(DeltaTime);
#if DEBUG
                }
                catch (System.Exception e)
                {
                    Debugger.CurrentDebugger.Log(e, LogLevel.Error);
                }
#endif
            }
        }
    }
}
