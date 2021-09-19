using Site13Kernel.Core;
using Site13Kernel.Data;
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
        public List<AudioSource> GunSFXSources;
        public GameObject BulletPrefab;
        public int EffectPrefab;
        public List<AudioClip> FireSounds = new List<AudioClip>();
        public float FireInterval = 0;
        public float Recoil = 0;
        public float MaxRecoil = 2;
        public float RecoilRecoverSpeed = 5;
        public BulletFireType BulletFireType;
        public WeaponFireType FireType = WeaponFireType.FullAuto;
        public float NonAutoCap = 1;

        public bool FIRE0 = false;
        public byte FIRE1 = 0;

        public SpherePosition RelativeEmissionPoint;
        public Transform FirePoint;
        public Transform EffectPoint;
        public Transform CurrentEffectPoint;
        float CountDown = 0;
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
                            if (FIRE1 == 0)
                                if (CountDown <= 0)
                                {
                                    FIRE1 = 1;
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
                        if (FIRE1 == 1)
                        {
                            SingleFire();
                            CountDown = FireInterval;
                            FIRE1 = 2;
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
        public void Fire()
        {
            FIRE0 = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unfire()
        {
            FIRE0 = false;
            FIRE1 = 0;
        }
        int SFXIndex = 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SingleFire()
        {
            if (BulletPrefab != null)
                GameRuntime.CurrentGlobals.CurrentBulletSystem.AddBullet(BulletPrefab, FirePoint.position, this.transform.rotation);
            if (EffectPrefab != -1)
            {
                GameRuntime.CurrentGlobals.CurrentEffectController.Spawn(EffectPrefab, CurrentEffectPoint.position, this.transform.rotation,Vector3.one,CurrentEffectPoint);
            }
            if(BulletFireType == BulletFireType.HitScan)
            {

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
