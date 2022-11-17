using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

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
        public bool ScaleControlled = false;
        public Image[] Graphics;
        public Vector3 BaseScale = new Vector3(1, 1, 1);
        public Vector3 MaxScale = new Vector3(1, 1, 1);
        float D = -1;
        public void SetColor(Color c)
        {
            foreach (var item in Graphics)
            {
                item.color = c;
            }
        }
        public override void Init()
        {
            D = MaxDistance - MinDistance;
        }
        public void UpdateCrosshair(float Recoil)
        {
            Controlled.anchoredPosition = InitialPosition + new Vector2(XContolled ? ((MinDistance + Recoil * D) * (XReversed ? -1 : 1)) : 0,
                YContolled ? ((MinDistance + Recoil * D) * (YReversed ? -1 : 1)) : 0);
            if (ScaleControlled)
            {
                Controlled.localScale = math.lerp(BaseScale, MaxScale, Recoil);
            }
        }
    }
}
