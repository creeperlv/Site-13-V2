using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic
{
    public class ControlledCrosshair : ControlledBehavior
    {
        public RectTransform Controlled;
        public float MaxDistance;
        public float MinDistance;
        public Vector2 InitialPosition;
        public bool XContolled;
        public bool XReversed;
        public bool YContolled;
        public bool YReversed;
        float D=-1;
        public override void Init()
        {
            D = MaxDistance - MinDistance;
        }
        public void UpdateCrosshair(float Recoil)
        {
            Controlled.anchoredPosition = InitialPosition + new Vector2(XContolled ? ((MinDistance + Recoil * D) * (XReversed ? -1 : 1)) : 0,
                YContolled ? ((MinDistance + Recoil * D )* (YReversed ? -1 : 1)) : 0);
        }
    }
}
