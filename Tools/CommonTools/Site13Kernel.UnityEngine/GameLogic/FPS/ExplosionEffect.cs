using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.Effects;
using Site13Kernel.Utilities;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class ExplosionEffect : BaseEffect
    {
        public ExplosionDefinition explosionDefinition;
        public DamagableEntity Cause;
        public bool isGrenadeExplosion;
        public int ExplosionID;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Init()
        {
            base.Init();
            if (explosionDefinition != null) Explode();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Explode()
        {

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionDefinition.Radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                var Distance = (explosionPos - hit.transform.position).magnitude;
                var shake = hit.GetComponentInChildren<CameraShakeEffect>();
                if (shake != null)
                {
                    float _shake = explosionDefinition.ShakePower * MathUtilities.InverseNegativeLerp(-explosionDefinition.Radius, explosionDefinition.Radius, Distance);
                    shake.SetShake(_shake, true, _shake, true, 70, 1, 0.05f);
                }
                if (rb != null)
                    rb.AddExplosionForce(explosionDefinition.Power, explosionPos, explosionDefinition.Radius, 0, ForceMode.Impulse);
                else
                {
                    var CC = hit.GetComponent<SimulatedRigidBodyOverCharacterController>();
                    if (CC != null)
                    {
                        CC.AddForce((CC.transform.position - explosionPos).normalized * explosionDefinition.Power * GameEnv.ExplosionIntensityOnSimulatedRigidBody *
                            (MathUtilities.InverseNegativeLerp(0, explosionDefinition.Radius, Distance)));
                    }
                }
                var hittable = hit.GetComponent<DamagableEntity>();
                if (hittable != null)
                {
                    if (Cause == null)
                        hittable.Damage(explosionDefinition.CentralDamage * (MathUtilities.InverseNegativeLerp(0, explosionDefinition.Radius, Distance)));
                    else hittable.Damage(explosionDefinition.CentralDamage * (MathUtilities.InverseNegativeLerp(0, explosionDefinition.Radius, Distance)),
                        new DamageDescription
                        {
                            Origin = Cause,
                            DamageOriginIntID = ExplosionID,
                            Type = (isGrenadeExplosion ? DamageType.Grenade : DamageType.Explosion)
                        });
                }
            }
        }
    }
}
