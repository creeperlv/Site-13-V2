
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.GameLogic.Physic;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class MeleeArea : MonoBehaviour
    {
        public float BaseDamage;
        public float Force = 1;
        public bool AllowBackstabDetection;
        public GameObject Holder;
        public BipedEntity OriginEntity;
        public bool isDetecting = false;
        public AudioSource HitNormal;
        public AudioSource HitDamagable;
        bool __audio_flag_0 = false;
        bool __audio_flag_1 = false;
        private void OnTriggerEnter(Collider collision)
        {
            Hit(collision.gameObject);
        }
        private void OnCollisionEnter(Collision collision)
        {
            Hit(collision.gameObject);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartDetection()
        {
            isDetecting = true;
            __audio_flag_0 = true;
            __audio_flag_1 = true;
            this.gameObject.SetActive(true);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StopDetection()
        {
            __audio_flag_0 = false;
            __audio_flag_1 = false;
            this.gameObject.SetActive(false);
            isDetecting = false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Hit(GameObject collision)
        {
            if (isDetecting)
            {
                byte __final_effect = 0;
                var DE = collision.GetComponent<DamagableEntity>();
                var PhyObj = collision.GetComponentInChildren<PhysicsObject>();
                var DEREF = collision.GetComponent<DamagableEntityReference>();
                if (PhyObj != null)
                {
                    PhyObj.Emitter = OriginEntity;
                }
                if (DEREF != null)
                {
                    DE = DEREF.Reference;
                }
                var RIG = collision.GetComponent<Rigidbody>();
                if (RIG == null)
                {
                    var CLD = collision.GetComponent<Collider>();
                    if (CLD != null)
                    {
                        RIG = CLD.attachedRigidbody;
                    }
                }
                if (RIG != null)
                {
                    var ForceDirection = (Holder.transform.position - collision.transform.position).normalized;
                    var _Force = -ForceDirection * Force;
                    RIG.AddForce(_Force, ForceMode.Impulse);
                }
                if (DE == null)
                {
                    var REF = collision.GetComponent<DamagableEntityReference>();
                    if (REF != null)
                    {
                        DE = REF.Reference;
                    }
                }
                if (DE != null)
                {
                    if (Holder != null)
                    {
                        if (DE.gameObject == Holder) return;

                        var fps = Holder.GetComponent<FPSController>();
                        if (fps != null)
                        {
                            fps.OnHit();
                        }
                    }
                    __final_effect = 1;

                    if (AllowBackstabDetection && DE.CanBeBackstabed)
                    {
                        Vector3 forward = DE.transform.forward;
                        Vector3 toOther = Holder.transform.position - DE.transform.position;
                        if (Vector3.Dot(forward, toOther) < 0)
                        {
                            DE.Die(new DamageDescription
                            {
                                Origin = OriginEntity,
                                DamageInformation = new DamageInformation
                                {
                                    DamageAmount = DE.CurrentHP,
                                    Type = DamageType.Melee
                                }
                            });
                        }
                        else
                        {
                            if (OriginEntity == null)
                                DE.Damage(BaseDamage);
                            else
                                DE.Damage(BaseDamage, new DamageDescription
                                {
                                    Origin = OriginEntity,
                                    DamageInformation = new DamageInformation
                                    {
                                        DamageAmount = BaseDamage,
                                        Type = DamageType.Melee
                                    }
                                });
                        }
                    }
                    else
                    {
                        if (OriginEntity == null)
                            DE.Damage(BaseDamage);
                        else
                            DE.Damage(BaseDamage, new DamageDescription
                            {
                                Origin = OriginEntity,
                                DamageInformation = new DamageInformation
                                {
                                    DamageAmount = BaseDamage,
                                    Type = DamageType.Melee
                                }
                            });
                    }
                }
                switch (__final_effect)
                {
                    case 0:
                        {
                            if (__audio_flag_0)
                            {
                                if (HitNormal != null)
                                {
                                    HitNormal.Play();
                                }
                                __audio_flag_0 = false;
                            }
                        }
                        break;
                    case 1:
                        {
                            if (__audio_flag_1)
                            {
                                if (HitDamagable != null)
                                {
                                    HitDamagable.Play();
                                }
                                __audio_flag_1 = false;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
