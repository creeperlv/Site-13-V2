using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Effects
{
    public class FadeEffect : BaseEffect
    {
        public List<Renderer> RS;
        public float FadeIn;
        public float FadeOut;
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            foreach (var item in RS)
            {

                var c = item.material.color;
                if (TimeD < FadeIn)
                {
                    c.a = Mathf.Lerp(0, 1, TimeD / FadeIn);
                }
                else if (TimeD > LifeTime - FadeOut)
                {
                    c.a = Mathf.Lerp(1, 0, (TimeD - LifeTime + FadeOut) / FadeOut);
                }
                else
                {
                    c.a = 1;
                }
                item.material.color = c;
            }
        }
    }
}
