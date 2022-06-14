using CLUNL.Data.Serializables.CheckpointSystem;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.Data.IO;
using Site13Kernel.Data.Serializables;
using Site13Kernel.Diagnostics;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class DamagableEntity : ControlledBehavior, ICheckpointData, IHittable,IContainsPureData
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
        public List<DeathReplacement> DeathReplacements = new List<DeathReplacement>();
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
            {
                if (collision.gameObject.GetComponent<DoNotCauseCollisionDamage>() != null) return;
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
            if (Died) return;
            Died = true;
            if (OnDie != null)
            {
                if (OnDie()) return;
            }
            try
            {
                if (DeathReplacements.Count > 0)
                {
                    foreach (var item in DeathReplacements)
                    {
                        DeathBodyGen(item);
                    }
                }
                else
                {
                    DeathBodyGen(new DeathReplacement { TargetPrefab = new PrefabReference { ID = DeathBodyReplacementID }, BodyType = deathBodyType });
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
                if (ControlledObject != null)
                {
                    Destroy(ControlledObject.gameObject);
                }
                else
                    Destroy(this.gameObject);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void DeathBodyGen(DeathReplacement DR)
        {
            var deathBodyType = DR.BodyType;
            var DeathBodyReplacementID = DR.TargetPrefab.ID;
            switch (deathBodyType)
            {
                case DeathBodyType.Entity:
                    {
                        if (Controller != null && DeathBodyReplacementID != -1)
                        {
                            if (ControlledObject != null)
                            {
                                Controller.Instantiate(DeathBodyReplacementID, ControlledObject.transform.position, ControlledObject.transform.rotation, ControlledObject.transform.parent);
                            }
                            else
                                Controller.Instantiate(DeathBodyReplacementID, this.transform.position, this.transform.rotation, this.transform.parent);
                        }
                        else
                        {
                            if (ControlledObject != null)
                            {
                                ObjectGenerator.Instantiate(DR.TargetPrefab, ControlledObject.transform.position, ControlledObject.transform.rotation, ControlledObject.transform.parent);
                            }
                            else
                                ObjectGenerator.Instantiate(DR.TargetPrefab, this.transform.position, this.transform.rotation, this.transform.parent);
                        }
                    }
                    break;
                case DeathBodyType.Effect:
                    {
                        if (ControlledObject != null)
                        {
                            EffectController.CurrentEffectController.Spawn(DR.TargetPrefab, ControlledObject.transform.position, ControlledObject.transform.rotation);
                        }
                        else
                            EffectController.CurrentEffectController.Spawn(DR.TargetPrefab, this.transform.position, this.transform.rotation);
                    }
                    break;
                case DeathBodyType.Regular:
                    {
                        if (ControlledObject != null)
                        {
                            ObjectGenerator.Instantiate(DR.TargetPrefab, ControlledObject.transform.position, ControlledObject.transform.rotation, ControlledObject.transform.parent);
                        }
                        else
                            ObjectGenerator.Instantiate(DR.TargetPrefab, this.transform.position, this.transform.rotation, this.transform.parent);
                    }
                    break;
                case DeathBodyType.Explosion:
                    {
                        GameObject effect;
                        if (ControlledObject != null)
                        {
                            effect = EffectController.CurrentEffectController.Spawn(DR.TargetPrefab, ControlledObject.transform.position, ControlledObject.transform.rotation);
                        }
                        else
                            effect = EffectController.CurrentEffectController.Spawn(DR.TargetPrefab, this.transform.position, this.transform.rotation);
                        effect.GetComponent<ExplosionEffect>().Explode();
                    }
                    break;
                default:
                    break;
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

        public IPureData ObtainData()
        {
            return new SerializableDamagableEntity { CurrentHP = CurrentHP };
        }

        public void ApplyData(IPureData data)
        {
            if(data is SerializableDamagableEntity SDE)
            {
                CurrentHP = SDE.CurrentHP;
            }
        }
    }
}
