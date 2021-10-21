using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class ChunkDifferentialFrameSyncController : BehaviorController
    {
        void Update()
        {
            float DeltaTime = Time.deltaTime;
            float UDeltaTime = Time.unscaledDeltaTime;
            foreach (var item in _OnRefresh)
            {
                if (INTERRUPT_REFRESH) continue;
                item.Refresh(DeltaTime, UDeltaTime);
            }
        }
    }
    public class ChunkDifferentialFrameSyncSubController : BehaviorController, IControllable
    {
        public int FRAME_IGNORANCE;
        public BaseController Parent { get; set; }

        public void FixedRefresh(float DeltaTime, float UnscaledDeltaTime)
        {
        }

        public void Init()
        {

        }
        public void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {

        }
    }
}
