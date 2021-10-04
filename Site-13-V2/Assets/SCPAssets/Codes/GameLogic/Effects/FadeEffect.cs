using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.Effects
{
    public class FadeEffect : BaseEffect
    {
        public List<Renderer> RS;
        public float FadeIn;
        public float FadeOut;
        public bool isRotationRandom = false;
        public Transform Root;
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;
        public float MinZ;
        public float MaxZ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Init()
        {
            base.Init();
            if (isRotationRandom)
            {
                Root.localRotation = Quaternion.Euler(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY), Random.Range(MinZ, MaxZ));
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
