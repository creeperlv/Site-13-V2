using Site13Kernel.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class ChunkDifferentialFrameSyncController : BehaviorController
    {
        public List<ChunkDifferentialFrameSyncSubController> subControllers;
        int FRAME_UPDATE = 0;
        int FRAME_FIXEDUPDATE = 0;
        void Update()
        {
            float DeltaTime = Time.deltaTime;
            float UDeltaTime = Time.unscaledDeltaTime;
            foreach (var item in subControllers)
            {
                if (INTERRUPT_REFRESH) continue;
                item.Refresh(DeltaTime, UDeltaTime, FRAME_UPDATE % subControllers.Count);
            }
            if (FRAME_UPDATE % subControllers.Count == 0 && FRAME_UPDATE != 0)
            {
                FRAME_UPDATE = 1;
            }
            else
            {
                FRAME_UPDATE++;
            }
        }
        void FixedUpdate()
        {
            float DeltaTime = Time.fixedDeltaTime;
            float UDeltaTime = Time.fixedUnscaledDeltaTime;
            foreach (var item in subControllers)
            {
                if (INTERRUPT_REFRESH) continue;
                item.FixedRefresh(DeltaTime, UDeltaTime, FRAME_FIXEDUPDATE % subControllers.Count);
            }
            if (FRAME_FIXEDUPDATE % subControllers.Count == 0 && FRAME_FIXEDUPDATE != 0)
            {
                FRAME_FIXEDUPDATE = 1;
            }
            else
            {
                FRAME_FIXEDUPDATE++;
            }
        }
    }
    public class ChunkDifferentialFrameSyncSubController : BehaviorController
    {
        public int ID;
        public BaseController Parent { get; set; }
        float FR_DT = 0;
        float FR_UDT = 0;
        float R_DT = 0;
        float R_UDT = 0;
        public void FixedRefresh(float DeltaTime, float UnscaledDeltaTime, int CURRENTFRAME)
        {
            FR_DT += DeltaTime;
            FR_UDT += UnscaledDeltaTime;
            if (CURRENTFRAME == ID)
            {
                foreach (var item in _OnFixedRefresh)
                {
                    try
                    {
                        item.FixedRefresh(FR_DT, FR_UDT);

                    }
                    catch (Exception e)
                    {
                        Debugger.CurrentDebugger.LogError(e);
                    }
                }
                FR_DT = 0;
                FR_UDT = 0;
            }
        }

        public void Init()
        {
            SerializeAll();
            foreach (var item in _OnInit)
            {
                item.Init();
            }
        }
        public void Refresh(float DeltaTime, float UnscaledDeltaTime, int CURRENTFRAME)
        {
            R_DT += DeltaTime;
            R_UDT += UnscaledDeltaTime;
            if (CURRENTFRAME == ID)
            {
                foreach (var item in _OnRefresh)
                {
                    try
                    {
                        item.Refresh(R_DT, R_UDT);

                    }
                    catch (Exception e)
                    {
                        Debugger.CurrentDebugger.LogError(e);
                    }
                }
                R_DT = 0;
                R_UDT = 0;
            }
        }
    }
}
