using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Effects
{
    public class HitEffect : BaseEffect
    {

        public List<RectTransform> ControlledHit;
        public CanvasGroup Overall;
        public float MaxDistance;
        public float MinDistance;
        public Vector2 InitialPosition;
        public bool XContolled;
        public bool XReversed;
        public bool YContolled;
        public bool YReversed;
        public float FadeIn;
        public float FadeOut;
        float D = -1;
        public override void Init()
        {
            D = MaxDistance - MinDistance;
            D /= this.LifeTime;
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            foreach (var Controlled in ControlledHit)
            {

                Controlled.anchoredPosition = InitialPosition + new Vector2(XContolled ? ((MinDistance + TimeD * D) * (XReversed ? -1 : 1)) : 0,
                    YContolled ? ((MinDistance + TimeD * D) * (YReversed ? -1 : 1)) : 0);
            }
            if (TimeD < FadeIn)
            {
                Overall.alpha = Mathf.Lerp(0, 1, TimeD / FadeIn);
            }
            else if (TimeD > LifeTime - FadeOut)
            {
                Overall.alpha = Mathf.Lerp(1, 0, (TimeD - LifeTime + FadeOut) / FadeOut);
            }
            else
            {
                Overall.alpha = 1;
            }
        }
    }
}
