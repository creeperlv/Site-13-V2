using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    /// <summary>
    /// Hitscan Bullet, ignore hit action on physics.
    /// </summary>
    public class StraightBullet : BaseBullet
    {

        public float Velocity;
        public bool CauseDamage;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Move(float DT, float UDT)
        {
            this.transform.Translate(Vector3.forward * Velocity * DT, Space.Self);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Hit(Collider collision)
        {
            if (CauseDamage)
                base.Hit(collision);
            else
            {

                ParentSystem.DestoryBullet(this);
                return;
            }
        }
    }
}
