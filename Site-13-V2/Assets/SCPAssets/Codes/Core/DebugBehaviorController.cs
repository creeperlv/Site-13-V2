using Site13Kernel.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class DebugBehaviorController : BehaviorController
    {
#if DEBUG
        void Start()
        {
            if (CrossScene)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            foreach (var item in OnInit)
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
            foreach (var item in OnInit)
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
            foreach (var item in OnRefresh)
            {
                try
                {
                    item.Refresh(DeltaTime);
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
            foreach (var item in OnFixedRefresh)
            {
                try
                {
                    item.FixedRefresh(DeltaTime);
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
