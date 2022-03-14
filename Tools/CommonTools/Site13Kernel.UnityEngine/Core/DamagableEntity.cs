using CLUNL.Data.Serializables.CheckpointSystem;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Diagnostics;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

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
        public bool isInvincible;

        public bool TakeCollisionDamage = true;
        public DeathBodyType deathBodyType = DeathBodyType.Entity;
        public int DeathBodyReplacementID = -1;
        public GameObject DeathBodyReplacementPrefab = null;
        public GameObject ControlledObject = null;

        public AudioSource HitSound;
        public EntityController Controller;
        /// <summary>
        /// First parameter: Damage amount. <br/>
        /// Second parameter: Damage on Shield. <br/>
        /// Third parameter: Damage on Health. <br/>
        /// Fourth parameter: Remaining Shield. <br/>
        /// Fifth parameter: Remaining Health. <br/>
        /// </summary>
        public Action<float, float, float, float, float> OnTakingDamage = null;
        public Action OnShieldDown = null;

        public float TimeToSelfDestruction = -1f;
        public float LowestKillPlane = -500f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Load(List<object> data)
        {
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnCollisionEnter(Collision collision)
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
            if (HitSound != null)
                HitSound.Play();
            if (isInvincible) return;
            CurrentHP = math.max(0, CurrentHP - V);
            if (CurrentHP <= 0)
            {
                Die();
                return;
            }
            if (OnTakingDamage != null)
                OnTakingDamage(V, 0, V, 0, CurrentHP);
        }

        /// <summary>
        /// If it returns true, it will breaks original Die function, means Destory and EntityController.Remove(this); will not be executed.
        /// </summary>
        public Func<bool> OnDie = null;
        bool Died = false;
        /// <summary>
        /// Kills the entity no matter how much the current health is.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Die()
        {
            if(Died) return;
            Died = true;
            if (OnDie != null)
            {
                if (OnDie()) return;
            }
            try
            {
                switch (deathBodyType)
                {
                    case DeathBodyType.Entity:
                        {
                            if (Controller != null)
                            {
                                if (DeathBodyReplacementID != -1)
                                    Controller.Instantiate(DeathBodyReplacementID, this.transform.position, this.transform.rotation, this.transform.parent);
                                else if (DeathBodyReplacementPrefab != null)
                                    Controller.Instantiate(DeathBodyReplacementPrefab, this.transform.position, this.transform.rotation, this.transform.parent);
                            }
                            else
                            {
                                ObjectGenerator.Instantiate(DeathBodyReplacementID, this.transform.position, this.transform.rotation, this.transform.parent);
                            }
                        }
                        break;
                    case DeathBodyType.Effect:
                        {
                            EffectController.CurrentEffectController.Spawn(DeathBodyReplacementID, this.transform.position, this.transform.rotation);
                        }
                        break;
                    case DeathBodyType.Regular:
                        {
                            ObjectGenerator.Instantiate(DeathBodyReplacementID, this.transform.position, this.transform.rotation, this.transform.parent);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Debugger.CurrentDebugger.Log(e);
            }
            if (Controller != null)
                Controller.DestroyEntity(this);
            else
            {
                Destroy(this.gameObject);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetName()
        {
            return name;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new virtual void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (TimeToSelfDestruction != -1f)
            {
                TimeToSelfDestruction -= DeltaTime;
                if (TimeToSelfDestruction <= 0)
                {
                    Die();
                }
            }
            if (transform.position.y < LowestKillPlane)
            {
                Die();
            }
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
