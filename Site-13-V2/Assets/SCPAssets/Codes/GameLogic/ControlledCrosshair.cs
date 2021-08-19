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
        public bool XContolled;
        public bool YContolled;
    }
}
