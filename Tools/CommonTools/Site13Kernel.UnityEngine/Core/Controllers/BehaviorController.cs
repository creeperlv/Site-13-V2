using Site13Kernel.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class BehaviorController : BaseController
    {
        public bool CrossScene = false;
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
                if (INTERRUPT_INIT) continue;
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
            float DeltaTime = Time.deltaTime;
            float UDeltaTime = Time.unscaledDeltaTime;
            for (int i = _OnRefresh.Count - 1; i >= 0; i--)
            {
                if (INTERRUPT_REFRESH) continue;
                var item = _OnRefresh[i];
#if LOOSE_ERROR
                try
                {
#endif
                    item.Refresh(DeltaTime, UDeltaTime);
#if LOOSE_ERROR
                }
                catch (System.Exception e)
                {
                    Debugger.CurrentDebugger.Log(e, LogLevel.Error);
                }
#endif
            }
        }
        private void FixedUpdate()
        {

            float DeltaTime = Time.fixedDeltaTime;
            float UDeltaTime = Time.fixedUnscaledDeltaTime;
            for (int i = _OnFixedRefresh.Count - 1; i >= 0; i--)
            {
                if (INTERRUPT_FIXED_REFRESH) continue;
                var item = _OnFixedRefresh[i];
#if LOOSE_ERROR
                try
                {
#endif
                    item.FixedRefresh(DeltaTime, UDeltaTime);
#if LOOSE_ERROR
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
