using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    [Serializable]
    public class BasicWeapon : ControlledBehavior
    {
        public Weapon Base;
        public float MaxHitScanDistance;
        public List<AudioSource> GunSFXSources;
        public GameObject BulletPrefab;
        public int EffectPrefab;
        public List<AudioClip> FireSounds = new List<AudioClip>();
        public float FireInterval = 0;
        public float Recoil = 0;
        public float SingleFireRecoil = 0.5f;
        public float MaxRecoil = 2;
        public float RecoilRecoverSpeed = 5;
        public float MaxScatterAngle = 0;
        public float MaxScatterAngleAimMode = 0;
        /// <summary>
        /// 0 - Normal
        /// 1 - Aim
        /// </summary>
        public int AimingMode = 0;
        public BulletFireType BulletFireType;
        public WeaponFireType FireType = WeaponFireType.FullAuto;
        public float NonAutoCap = 1;//3 for BR55/75-Like weapon

        public bool FIRE0 = false;
        public bool FIRE3 = false;
        public byte FIRE1 = 0;
        public byte FIRE2 = 0; //Semi Auto Use

        public SpherePosition RelativeEmissionPoint;
        public Transform FirePoint;
        public Transform EffectPoint;
        public Transform CurrentEffectPoint;
        float CountDown = 0;
        float SemiCountDown = 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnFrame(float DeltaT, float UnscaledDeltaT)
        {
            if (FIRE0)
            {
                //Determine will be able to fire.
                switch (FireType)
                {
                    case WeaponFireType.FullAuto:
                        {
                            if (CountDown <= 0)
                            {
                                FIRE1 = 1;
                            }
                        }
                        break;
                    case WeaponFireType.SemiAuto:
                        {
                            if (FIRE3)
                                if (FIRE1 == 0)
                                    if (CountDown <= 0)
                                    {
                                        //CountDown = FireInterval;
                                        SemiCountDown = NonAutoCap;
                                        FIRE1 = 1;
                                        FIRE2 = 1;
                                    }
                        }
                        break;
                    case WeaponFireType.Heat:
                        break;
                    default:
                        break;
                }
            }
            CountDown -= DeltaT;
            {
                switch (FireType)
                {
                    case WeaponFireType.FullAuto:
                        {
                            if (FIRE1 == 1)
                            {
                                SingleFire();
                                CountDown = FireInterval;
                                FIRE1 = 0;
                            }
                        }
                        break;
                    case WeaponFireType.SemiAuto:
                        if (FIRE2 == 1)
                        {
                            SemiFireProgress(DeltaT, UnscaledDeltaT);
                        }
                        break;
                    case WeaponFireType.Heat:
                        break;
                    default:
                        break;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SemiFireProgress(float DeltaTime, float UnscaledDeltaTime)
        {
            if (SemiCountDown > 0)
            {

                CountDown -= DeltaTime;
                if (CountDown <= 0)
                {
                    SingleFire();
                    CountDown = FireInterval;
                    SemiCountDown--;
                }
            }
            else
            {
                FIRE2 = 0;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fire()
        {
            FIRE0 = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Press()
        {
            FIRE3 = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unfire()
        {
            FIRE0 = false;
            FIRE3 = false;
            FIRE1 = 0;
        }
        int SFXIndex = 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SingleFire()
        {
            Recoil = Math.Min(Recoil + SingleFireRecoil, MaxRecoil);
            Quaternion Rotation = this.transform.rotation;
            Vector3 RecoilAngle = MathUtilities.RandomDirectionAngleOnXYAndZ0(Recoil / MaxRecoil * (AimingMode == 0 ? MaxScatterAngle : MaxScatterAngleAimMode), Camera.main.fieldOfView);
            Vector3 RecoilAngle2 = MathUtilities.RandomDirectionAngleOnXYAndZ1(Recoil / MaxRecoil * (AimingMode == 0 ? MaxScatterAngle : MaxScatterAngleAimMode));
            //Debug.Log(RecoilAngle);

            {
                Vector3 V = Rotation.eulerAngles;
                V += RecoilAngle;
                Rotation = Quaternion.Euler(V);
            }
            if (BulletPrefab != null)
                GameRuntime.CurrentGlobals.CurrentBulletSystem.AddBullet(BulletPrefab, FirePoint.position, Rotation);
            if (EffectPrefab != -1)
            {
                GameRuntime.CurrentGlobals.CurrentEffectController.Spawn(EffectPrefab, CurrentEffectPoint.position, Rotation, Vector3.one, CurrentEffectPoint);
            }
            if (BulletFireType == BulletFireType.HitScan)
            {
                Vector3 _Rotation = FirePoint.forward;
                {
                    _Rotation += FirePoint.TransformDirection(RecoilAngle2);
                    //Rotation = Quaternion.Euler(V);
                    Debug.Log(_Rotation);
                    Debug.DrawRay(FirePoint.position, _Rotation, Color.green, 5);
                }
                Physics.Raycast(FirePoint.position, _Rotation, out var info, MaxHitScanDistance);
                if (info.collider != null)
                {
                    {
                        var Hittable = info.collider.GetComponent<IHittable>();

                        if (Hittable != null)
                        {
                            GameRuntime.CurrentGlobals.CurrentEffectController.Spawn(Hittable.HitEffectHashCode(), info.point, Quaternion.identity, Vector3.one);
                        }
                        else
                        {
                            GameRuntime.CurrentGlobals.CurrentEffectController.Spawn(1, info.point, Quaternion.identity, Vector3.one);

                        }
                        var Entity = info.collider.GetComponent<DamagableEntity>();
                        var WeakPoint = info.collider.GetComponent<WeakPoint>();
                        if (WeakPoint != null)
                        {
                            WeakPoint.AttachedBioEntity.Damage(BulletPrefab.GetComponent<BaseBullet>().WeakPointDamage);
                        }
                        else if (Entity != null)
                        {
                            Entity.Damage(BulletPrefab.GetComponent<BaseBullet>().BaseDamage);
                        }

                    }
                }
            }
            GunSFXSources[SFXIndex].Stop();
            GunSFXSources[SFXIndex].clip = FireSounds[UnityEngine.Random.Range(0, FireSounds.Count)];
            GunSFXSources[SFXIndex].Play();
            SFXIndex++;
            if (SFXIndex == GunSFXSources.Count)
            {
                SFXIndex = 0;
            }
        }
    }
}
