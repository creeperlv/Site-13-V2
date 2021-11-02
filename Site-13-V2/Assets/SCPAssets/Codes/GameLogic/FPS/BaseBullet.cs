using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.GameLogic.FPS;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class BaseBullet : ControlledBehavior
    {
        public float BaseDamage = 50;
        public float WeakPointDamage = 50;
        public float LifeTime = 10;
        public GameObject Emitter;
        //public Collider c;
        [HideInInspector]
        public BulletSystem ParentSystem { get; set; }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {

            //c.attachedRigidbody.collisionDetectionMode= CollisionDetectionMode.
            LifeTime -= DeltaTime;
            Move(DeltaTime, UnscaledDeltaTime);
            if (LifeTime < 0)
            {
                ParentSystem.DestoryBullet(this);
            }
        }
        public virtual void Move(float DT, float UDT)
        {
        }
        private void OnCollisionEnter(Collision collision)
        {
        }
        private void OnTriggerEnter(Collider collision)
        {
            Hit(collision);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Hit(Collider collision)
        {
            var Hittable = collision.gameObject.GetComponent<IHittable>();

            if (Hittable != null)
            {
                EffectController.CurrentEffectController.Spawn(Hittable.HitEffectHashCode(), collision.ClosestPoint(transform.position), Quaternion.identity, Vector3.one);
            }
            else
            {
                EffectController.CurrentEffectController.Spawn(1, collision.ClosestPoint(transform.position), Quaternion.identity, Vector3.one);

            }
            var Entity = collision.gameObject.GetComponent<DamagableEntity>();
            var WeakPoint = collision.gameObject.GetComponent<WeakPoint>();
            if (WeakPoint != null)
            {
                TrySpawnHitEffect();
                WeakPoint.AttachedBioEntity.Damage(WeakPointDamage);
            }
            else if (Entity != null)
            {
                TrySpawnHitEffect();
                Entity.Damage(BaseDamage);
            }

            ParentSystem.DestoryBullet(this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TrySpawnHitEffect()
        {
            if (Emitter != null)
            {
                var FPS = Emitter.GetComponent<FPSController>();
                if (FPS != null)
                {
                    FPS.OnHit();
                }
            }
        }
    }
}
