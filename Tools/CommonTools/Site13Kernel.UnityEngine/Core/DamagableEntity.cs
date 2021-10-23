using CLUNL.Data.Serializables.CheckpointSystem;
using Site13Kernel.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Site13Kernel.Core
{
    public class DamagableEntity : ControlledBehavior, ICheckpointData, IHittable
    {
        public string Name;

        public string ProtoTypeID;

        public float MaxHP;
        public float InitHP;
        public float CurrentHP;
        public int HitEffect;
        public bool CanBeBackstabed;

        public bool TakeCollisionDamage = true;

        public EntityController Controller;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Load(List<object> data)
        {

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnCollisionEnter(UnityEngine.Collision collision)
        {
            if (TakeCollisionDamage)
                if (collision.rigidbody != null)
                {
                    var V = collision.relativeVelocity;
                    float v = V.magnitude;
                    if (v > GameEnv.CollisionDamageSpeedThreshold)
                    {
                        var DELTA = v - GameEnv.CollisionDamageSpeedThreshold;
                        Damage(DELTA * GameEnv.CollisionDamageIntensity);
                    }
                }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Init()
        {
            CurrentHP = InitHP;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Damage(float V)
        {
            CurrentHP = math.max(0, CurrentHP - V);
            if (CurrentHP <= 0)
                Die();
        }

        /// <summary>
        /// If it returns true, it will breaks original Die function, means Destory and EntityController.Remove(this); will not be executed.
        /// </summary>
        public Func<bool> OnDie = null;
        /// <summary>
        /// Kills the entity no matter how much the current health is.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Die()
        {
            if (OnDie != null)
            {
                if (OnDie()) return;
            }
            Controller.DestroyEntity(this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetName()
        {
            return name;
        }
        public List<object> Save()
        {
            var L = new List<object> { ProtoTypeID,
                MaxHP,
                CurrentHP };
            L.AddRange(SubSave());
            return L;
        }
        public virtual List<object> SubSave()
        {
            return new List<object>();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int HitEffectHashCode()
        {
            return HitEffect;
        }
    }
}
