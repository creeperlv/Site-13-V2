using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class ExplodableBullet : BaseBullet
    {

        public float Velocity;
        public int HitEffect = -1;
        public ExplosionDefinition Damage;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Move(float DT, float UDT)
        {
            this.transform.Translate(Vector3.forward * Velocity * DT, Space.Self);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Hit(Collider collision)
        {

            var Hittable = collision.gameObject.GetComponent<IHittable>();


            if (Hittable != null)
            {
                if (((MonoBehaviour)Hittable).gameObject == Emitter)
                {
                    return;
                }
            } 
                {
                var Effect=EffectController.CurrentEffectController.Spawn(HitEffect, collision.ClosestPoint(transform.position), Quaternion.identity, Vector3.one);
                var __exp = Effect.GetComponent<ExplosionEffect>();
                if(__exp != null)
                {
                    __exp.explosionDefinition = Damage;
                    __exp.Explode();
                }
                ParentSystem.DestoryBullet(this);
                return;
            }
        }
    }
}
