using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
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
        public int HitEffect = -1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Move(float DT, float UDT)
        {
            this.transform.Translate(Vector3.forward * Velocity * DT, Space.Self);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AlteredHit(Collider collision)
        {
            var Hittable = collision.gameObject.GetComponent<IHittable>();


            if (Hittable != null)
            {
                if (((MonoBehaviour)Hittable).gameObject == Emitter)
                {
                    return;
                }
                var Effect = EffectController.CurrentEffectController.Spawn(Hittable.HitEffectHashCode(), collision.ClosestPoint(transform.position), Quaternion.identity, Vector3.one);
            }
            else
            {
                EffectController.CurrentEffectController.Spawn(1, collision.ClosestPoint(transform.position), Quaternion.identity, Vector3.one);

            }
            if (HitEffect != -1)
                EffectController.CurrentEffectController.Spawn(HitEffect, collision.ClosestPoint(transform.position), Quaternion.identity, Vector3.one);
            var Entity = collision.gameObject.GetComponent<DamagableEntity>();
            var EntityREF = collision.gameObject.GetComponent<DamagableEntityReference>();
            if(EntityREF != null)
            {
                Entity = EntityREF.Reference;
            }
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
        public override void Hit(Collider collision)
        {
            if (CauseDamage)
                AlteredHit(collision);
            else
            {

                ParentSystem.DestoryBullet(this);
                return;
            }
        }
    }
}
