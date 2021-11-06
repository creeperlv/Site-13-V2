using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.Effects;
using System;
using System.Collections.Generic;
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
        float TimeD;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            TimeD += DeltaTime;
            if (TimeD >= baseGrenade.DetonationDuration)
            {
                var effect = EffectController.CurrentEffectController.Spawn(baseGrenade.EffectHashCode, this.transform.position, this.transform.rotation, EffectController.CurrentEffectController.transform);
                effect.GetComponent<ExplosionEffect>().explosionDefinition = baseGrenade.Explosion;
                effect.GetComponent<ExplosionEffect>().Explode();
                ParentController.DestoryGrenade(this);
            }
        }

    }
    public class ExplosionEffect : BaseEffect
    {
        public ExplosionDefinition explosionDefinition;
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

                if (rb != null)
                    rb.AddExplosionForce(explosionDefinition.Power, explosionPos, explosionDefinition.Radius, 0, ForceMode.Impulse);
                var hittable = hit.GetComponent<IHittable>();
                if (hittable != null)
                {

                }
            }
        }
    }
    /// <summary>
    /// Suggested to be ran in ChunkDifferentialFrameSyncController.
    /// </summary>
    public class GrenadeController : ControlledBehavior
    {
        public List<ControlledGrenade> ControlledGrenades = new List<ControlledGrenade>();
        public static GrenadeController CurrentController;
        public override void Init()
        {
            CurrentController = this;
            Parent.RegisterRefresh(this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, Vector3 ForceDirection, ForceMode forceMode)
        {
            var GO = GameObject.Instantiate(gameObject);
            GO.transform.position = Position;
            GO.transform.rotation = Rotation;
            GO.GetComponent<Rigidbody>().AddForce(ForceDirection, forceMode);
            var GRE = GO.GetComponent<ControlledGrenade>();
            GRE.ParentController = this;
            ControlledGrenades.Add(GRE);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            for (int i = ControlledGrenades.Count - 1; i >= 0; i--)
            {
                var item = ControlledGrenades[i];
                if (item != null)
                    item.Refresh(DeltaTime, UnscaledDeltaTime);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestoryGrenade(ControlledGrenade CG)
        {
            ControlledGrenades.Remove(CG);
            Destroy(CG.gameObject);
        }
    }
}
