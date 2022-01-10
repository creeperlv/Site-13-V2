using Site13Kernel.Core;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Site13Kernel.GameLogic
{
    public class DedicatedCamera : ControlledBehavior
    {
        public Transform TargetCam;
        public float MinH;
        public float MaxH;
        public float MinV;
        public float MaxV;
        public float IntensityH = 1;
        public float IntensityV = 1;
        public override void Init()
        {
            Parent.RegisterRefresh(this);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            var H = Inputs.GetAxis("MouseH");
            var V = Inputs.GetAxis("MouseV");
            var EA = TargetCam.localRotation.eulerAngles;
            var __H = Mathf.Clamp((EA.y > 180 ? EA.y - 360 : EA.y) + H * IntensityH, MinH, MaxH);
            if (__H < 0)
            {
                __H += 360;
            }
            var __V = Mathf.Clamp((EA.x > 180 ? EA.x - 360 : EA.x) + V * IntensityV, MinV, MaxV);
            if (__V < 0)
            {
                __V += 360;
            }
            EA.y = __H;
            EA.x = __V;
            TargetCam.localRotation = Quaternion.Euler(EA);

        }
    }
}
