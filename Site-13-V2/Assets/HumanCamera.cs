using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class HumanCamera : ControlledBehavior
    {
        public float BaseFov;
        public float BreathIntensity;
        public float BreathSpeed;
        public float BreathCycle;
        public float BreathProgress;

        public Camera ControlledCamera;
        public override void Init()
        {
            Parent.RegisterRefresh(this);
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            BreathProgress += DeltaTime * BreathSpeed;
            ControlledCamera.fieldOfView = BaseFov + Mathf.Sin(BreathProgress ) * BreathIntensity;
            if (BreathProgress > BreathCycle)
                BreathCycle = 0;
        }
    }
}
