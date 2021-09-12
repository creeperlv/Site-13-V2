using Site13Kernel.Core;
using Site13Kernel.GameLogic.FPS;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class BaseBullet : ControlledBehavior
    {
        public float BaseDamage= 50;
        public float WeakPointDamage = 50;
        public new BulletSystem Parent { get; set; }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            Move(DeltaTime, UnscaledDeltaTime);
        }
        public virtual void Move(float DT, float UDT)
        {
        }
        private void OnCollisionEnter(Collision collision)
        {
            Hit(collision);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Hit(Collision collision)
        {
            var Entity = collision.gameObject.GetComponent<BioEntity>();
            var WeakPoint = collision.gameObject.GetComponent<WeakPoint>();
            if (WeakPoint != null)
            {
                WeakPoint.AttachedBioEntity.Damage(WeakPointDamage);
            }
            else if (Entity != null)
            {
                Entity.Damage(BaseDamage);
            }
            Parent.DestoryBullet(this);
        }
    }
}
