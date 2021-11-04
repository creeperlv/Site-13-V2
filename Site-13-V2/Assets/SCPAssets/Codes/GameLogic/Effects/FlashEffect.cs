using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.Effects
{
    public class FlashEffect : BaseEffect
    {
        public List<GameObject> Objects;
        public float FlashLength;
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
            foreach (var item in Objects)
            {
                if (TimeD < FlashLength)
                {
                    if (item.activeSelf == false) item.SetActive(true);
                }
                else
                {

                    if (item.activeSelf == true) item.SetActive(false);
                }
            }
        }
    }
}
