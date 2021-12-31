using Site13Kernel.Animations;
using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
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
        public AudioSource MeleeSFXSource;
        public AudioClip MeleeSFX;
        public AudioSource ReloadSFXSource;
        public AudioClip ReloadSFX;
        public GameObject BulletPrefab;
        public Animator ControlledAnimator;
        public CompatibleAnimator CCAnimator;
        public string FireAnime = "Fire";
        public int Fire_HashCode = 5;
        public string TakeOut = "TakeOut";
        public int TakeOut_HashCode = 1;
        public float TakeOut_Length = 1;
        public float TakeOut_Length_D = 0;
        public string Combat_Trigger = "Combat";
        public int Combat_HashCode = 2;
        public string ReloadTrigger = "Reload";
        public int Reload_HashCode = 3;
        public float ReloadP0;
        public float ReloadP1;
        public float ReloadCountDown;
        public float CombatLength;
        public float CombatT;
        public string Idle = "Idle";
        public int Idle_HashCode = 4;
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

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="WeaponConstants"/>
        public int WeaponMode = WeaponConstants.WEAPON_MODE_NORMAL;

        public bool FIRE0 = false;
        public bool FIRE3 = false;
        public byte FIRE1 = 0;
        public byte FIRE2 = 0; //Semi Auto Use
        public SpherePosition RelativeEmissionPoint;
        public Transform FirePoint;
        public Transform EffectPoint;
        public Transform CurrentEffectPoint;
        public bool isHoldByPlayer = false;
        float CountDown = 0;
        float SemiCountDown = 0;
        int Mode = 0;
        public float MeleePoint;
        public MeleeArea MeleeArea;

        public Action OnHit;

        public GameObject ActualHolder = null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetProcessor(GameObject Processor)
        {
            MeleeArea.Holder = Processor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetFireMode(int V)
        {
            Mode = V;
            if (V == 0)
            {

                FireType = Base.WeaponFireType0;
            }
            else
            {

                FireType = Base.WeaponFireType1;
            }
        }
        /// <summary>
        /// Melee, in other words.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Combat()
        {
            Debugger.CurrentDebugger.Log("Meleeeeee?");
            if (WeaponMode == WeaponConstants.WEAPON_MODE_NORMAL)
            {
                Debugger.CurrentDebugger.Log("Meleeeeee!");
                CombatT = 0;
                WeaponMode = WeaponConstants.WEAPON_MODE_MELEE;
                if (MeleeSFXSource != null)
                {
                    MeleeSFXSource.clip = MeleeSFX;
                    MeleeSFXSource.Play();
                }
                if (CCAnimator != null)
                {
                    CCAnimator.SetAnimation(Combat_HashCode);
                }
                if (MeleeArea != null)
                {
                    MeleeArea.StartDetection();
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CancelCombat()
        {
            if (WeaponMode == WeaponConstants.WEAPON_MODE_MELEE)
            {
                WeaponMode = WeaponConstants.WEAPON_MODE_NORMAL;
                if (MeleeArea != null)
                {
                    MeleeArea.StopDetection();
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanReload()
        {

            if (WeaponMode == WeaponConstants.WEAPON_MODE_NORMAL)
            {
                if (Base.CurrentMagazine < Base.MagazineCapacity && Base.CurrentBackup > 0)
                {
                    return true;
                }
            }
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Reload()
        {
            if (WeaponMode == WeaponConstants.WEAPON_MODE_NORMAL)
            {
                if (Base.CurrentMagazine < Base.MagazineCapacity && Base.CurrentBackup > 0)
                {
                    ReloadSFXSource.clip = ReloadSFX;
                    ReloadSFXSource.Play();
                    ReloadCountDown = ReloadP0 + ReloadP1;
                    WeaponMode = WeaponConstants.WEAPON_MODE_RELOAD_STAGE_0;
                    if (CCAnimator != null)
                    {
                        CCAnimator.SetAnimation(Reload_HashCode);
                    }
                    return true;
                }
            }
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRecoilMax(float V)
        {
            Recoil = Mathf.Max(Recoil, V);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnFrame(float DeltaT, float UnscaledDeltaT)
        {
            if (WeaponMode == WeaponConstants.WEAPON_MODE_NORMAL)
            {
                //Fire.
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
                                    if (Fire_HashCode != -1)
                                    {
                                    }
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
            else if (WeaponMode == WeaponConstants.WEAPON_MODE_RELOAD_STAGE_0)
            {
                ReloadCountDown -= DeltaT;
                if (ReloadCountDown < ReloadP1)
                {
                    var TOTAL = Base.CurrentBackup + Base.CurrentMagazine;
                    if (TOTAL > Base.MagazineCapacity)
                    {
                        Base.CurrentMagazine = Base.MagazineCapacity;
                        Base.CurrentBackup = TOTAL - Base.MagazineCapacity;
                    }
                    else
                    {
                        Base.CurrentMagazine = TOTAL;
                        Base.CurrentBackup = 0;
                    }
                    WeaponMode = WeaponConstants.WEAPON_MODE_RELOAD_STAGE_1;
                }
            }
            else if (WeaponMode == WeaponConstants.WEAPON_MODE_RELOAD_STAGE_1)
            {

                ReloadCountDown -= DeltaT;
                if (ReloadCountDown <= 0)
                {
                    WeaponMode = WeaponConstants.WEAPON_MODE_NORMAL;
                }
            }
            else if (WeaponMode == WeaponConstants.WEAPON_MODE_MELEE)
            {
                CombatT += DeltaT;
                if (CombatT > MeleePoint)
                {
                    MeleeArea.StopDetection();
                }
                if (CombatT > CombatLength)
                {
                    CombatT = 0;
                    CancelCombat();
                }
            }
            else if (WeaponMode == WeaponConstants.WEAPON_MODE_TAKEOUT)
            {
                TakeOut_Length_D += DeltaT;
                if (TakeOut_Length_D >= TakeOut_Length)
                {
                    WeaponMode = WeaponConstants.WEAPON_MODE_NORMAL;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetTakeOut()
        {
            TakeOut_Length_D = 0;
            WeaponMode = WeaponConstants.WEAPON_MODE_TAKEOUT;
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
        Action<float> OnCurrentMagChanged = null;
        /// <summary>
        /// Only Applies to non-laser weapons.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SingleFire()
        {
            if (Base.CurrentMagazine > 0 || isHoldByPlayer == false)
            {
                if (FireType == WeaponFireType.FullAuto || FireType == WeaponFireType.SemiAuto)
                {
                    if (isHoldByPlayer)
                        this.Base.CurrentMagazine--;
                    if (OnCurrentMagChanged != null)
                    {
                        OnCurrentMagChanged(Base.CurrentMagazine);
                    }
                    Recoil = Math.Min(Recoil + SingleFireRecoil, MaxRecoil);
                    Quaternion Rotation = this.transform.rotation;
                    Vector3 RecoilAngle = MathUtilities.RandomDirectionAngleOnXYAndZ0(Recoil / MaxRecoil * (AimingMode == 0 ? MaxScatterAngle : MaxScatterAngleAimMode), Camera.main.fieldOfView);
                    Vector3 RecoilAngle2 = MathUtilities.RandomDirectionAngleOnXYAndZ1(Recoil / MaxRecoil * (AimingMode == 0 ? MaxScatterAngle : MaxScatterAngleAimMode));
                    //Debug.Log(RecoilAngle);

                    if (CCAnimator != null)
                        CCAnimator.SetAnimation(Fire_HashCode);
                    {
                        Vector3 V = Rotation.eulerAngles;
                        V += RecoilAngle;
                        Rotation = Quaternion.Euler(V);
                    }
                    if (BulletPrefab != null)
                        GameRuntime.CurrentGlobals.CurrentBulletSystem.AddBullet(BulletPrefab, FirePoint.position, Rotation, ActualHolder);
                    if (EffectPrefab != -1)
                    {
                        var GO = GameRuntime.CurrentGlobals.CurrentEffectController.Spawn(EffectPrefab, CurrentEffectPoint.position, this.transform.rotation, Vector3.one, CurrentEffectPoint);
                        GO.layer = this.gameObject.layer;
                    }
                    if (BulletFireType == BulletFireType.HitScan)
                    {
                        Vector3 _Rotation = FirePoint.forward;
                        {
                            _Rotation += FirePoint.TransformDirection(RecoilAngle2);
                        }
                        RaycastHit info;
                        if (!isHoldByPlayer)
                            Physics.Raycast(FirePoint.position, _Rotation, out info, MaxHitScanDistance, GameRuntime.CurrentGlobals.LayerExcludeAirBlock, QueryTriggerInteraction.Ignore);
                        else Physics.Raycast(FirePoint.position, _Rotation, out info, MaxHitScanDistance, GameRuntime.CurrentGlobals.LayerExcludePlayerAndAirBlock, QueryTriggerInteraction.Ignore);
                        if (info.collider != null)
                        {
                            if (info.collider.attachedRigidbody != null)
                            {
                                info.collider.attachedRigidbody.AddForce(_Rotation.normalized * Base.PhysicsForce, ForceMode.Impulse);

                            }

                            {
                                var Hittable = info.collider.GetComponent<IHittable>();

                                Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, info.normal);
                                if (Hittable != null)
                                {
                                    GameRuntime.CurrentGlobals.CurrentEffectController.Spawn(Hittable.HitEffectHashCode(), info.point, quaternion, Vector3.one, info.collider.transform, true);
                                }
                                else
                                {
                                    GameRuntime.CurrentGlobals.CurrentEffectController.Spawn(1, info.point, quaternion, Vector3.one, info.collider.transform, true);

                                }
                                var Entity = info.collider.GetComponent<DamagableEntity>();
                                var WeakPoint = info.collider.GetComponent<WeakPoint>();
                                if (WeakPoint != null)
                                {
                                    if (OnHit != null)
                                    {
                                        OnHit();
                                    }
                                    WeakPoint.AttachedBioEntity.Damage(BulletPrefab.GetComponent<BaseBullet>().WeakPointDamage);
                                }
                                else if (Entity != null)
                                {
                                    if (OnHit != null)
                                    {
                                        OnHit();
                                    }
                                    Entity.Damage(BulletPrefab.GetComponent<BaseBullet>().BaseDamage);
                                }

                            }
                        }
                    }
                    SFXIndex = UnityEngine.Random.Range(0, GunSFXSources.Count);
                    GunSFXSources[SFXIndex].Stop();
                    GunSFXSources[SFXIndex].clip = FireSounds[UnityEngine.Random.Range(0, FireSounds.Count)];
                    GunSFXSources[SFXIndex].Play();
                    //SFXIndex++;
                    //if (SFXIndex == GunSFXSources.Count)
                    //{
                    //    SFXIndex = 0;
                    //}

                }
            }
            else
            {
            }

        }
    }
}
