using Site13Kernel.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = Site13Kernel.Diagnostics.Debug;

namespace Site13Kernel.Core.Controllers
{
    public class DebugBehaviorController : BehaviorController
    {
#if DEBUG
        void Start()
        {
            GameRuntime.CurrentLocals = new GameLocals();
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
                    Debug.LogError(e);
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
                    Debug.LogError(e);
                }
            }
        }
        void Update()
        {
            float DeltaTime=Time.deltaTime;
            float UDeltaTime=Time.unscaledDeltaTime;
            foreach (var item in _OnRefresh)
            {
                try
                {
                    item.Refresh(DeltaTime,UDeltaTime);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        private void FixedUpdate()
        {

            float DeltaTime=Time.fixedDeltaTime;
            float UDeltaTime=Time.fixedUnscaledDeltaTime;
            foreach (var item in _OnFixedRefresh)
            {
                try
                {
                    item.FixedRefresh(DeltaTime,UDeltaTime);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
#else
    void Start(){
        GameObject.Destroy(this.gameObject);
    }
#endif
    }
}
