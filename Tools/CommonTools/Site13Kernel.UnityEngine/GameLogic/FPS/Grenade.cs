using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class ControlledGrenade : ControlledBehavior
    {
        public BaseGrenade baseGrenade;
        public Rigidbody Rigidbody;
        public GrenadeController ParentController;
        public DamagableEntity Cause;
        float TimeD;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            TimeD += DeltaTime;
            if (TimeD >= baseGrenade.DetonationDuration)
            {
                var effect = EffectController.CurrentEffectController.Spawn(baseGrenade.EffectHashCode, this.transform.position, this.transform.rotation, Vector3.one, EffectController.CurrentEffectController.transform);
                var ee = effect.GetComponent<ExplosionEffect>();
                ee.Cause = Cause;
                ee.explosionDefinition = baseGrenade.Explosion;
                ee.ExplosionID = baseGrenade.GrenadeHashCode;
                ee.isGrenadeExplosion = true;
                ee.Explode();
                ParentController.DestoryGrenade(this);
            }
        }
    }
}
