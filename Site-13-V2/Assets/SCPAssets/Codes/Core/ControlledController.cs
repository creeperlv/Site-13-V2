using Site13Kernel.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class ControlledController : BaseController,IControllable
    {
        public bool CrossScene=false;

        public BaseController Parent
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        public void Init()
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

        public void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            foreach (var item in _OnRefresh)
            {
#if DEBUG
                try
                {
#endif
                    item.Refresh(DeltaTime,UnscaledDeltaTime);
#if DEBUG
                }
                catch (System.Exception e)
                {
                    Debugger.CurrentDebugger.Log(e, LogLevel.Error);
                }
#endif
            }
        }

        public void FixedRefresh(float DeltaTime, float UnscaledDeltaTime)
        {
            foreach (var item in _OnFixedRefresh)
            {
#if DEBUG
                try
                {
#endif
                    item.FixedRefresh(DeltaTime,UnscaledDeltaTime);
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
