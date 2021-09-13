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
        public float LifeTime = 10;
        public new BulletSystem Parent { get; set; }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            LifeTime -= DeltaTime;
            Move(DeltaTime, UnscaledDeltaTime);
            if (LifeTime < 0)
            {
                Parent.DestoryBullet(this);
            }
        }
        public virtual void Move(float DT, float UDT)
        {
        }
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Hit_C");
        }
        private void OnTriggerEnter(Collider collision)
        {
            Debug.Log("Hit_T");
            Hit(collision);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Hit(Collider collision)
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
