using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class StraightBullet : BaseBullet
    {

        public float Velocity;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Move(float DT, float UDT)
        {
            this.transform.Translate(Vector3.forward * Velocity * DT, Space.Self);
        }
    }
}
