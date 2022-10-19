using Site13Kernel.Animations;
using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.Effects;
using Site13Kernel.GameLogic.Level;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static FullScreenPassRendererFeature;

namespace Site13Kernel.GameLogic.FPS
{
    [Serializable]
    public class GenericWeapon : ControlledBehavior
    {
        public Weapon WeaponData;
        public string AnimationCollectionName;
        public int CrossHairID;
        public WrappedAnimator WeaponAnimation;
        public string Trigger_Idle;
        public string Trigger_Fire;
        public string Trigger_Overheat;
        public string Trigger_Vent;
        public AmmoDisp AmmoDispType = AmmoDisp.None;
        public List<Renderer> AmmoRenderers;
        public List<Text> AmmoDispTexts;
        public enum AmmoDisp
        {
            None, TwoDig, ThreeDig, Liner, Text
        }
        public bool isHoldByPlayer = false;
        /// <summary>
        /// 0 - Normal
        /// 1 - Aim
        /// </summary>
        public int AimingMode = 0;
        public float MaxHitScanDistance;
        public WeaponFireType FireType = WeaponFireType.FullAuto;
        public BulletFireType BulletFireType = BulletFireType.HitScan;
        [Header("Bullet")]
        public PrefabReference Bullet;
        public PrefabReference EffectPrefab;
        public PrefabReference BulletHitEffect;
        public PrefabReference EjectionPrefab;
        [Header("Effects")]
        public Transform FirePoint;
        public Transform CurrentFirePoint;
        public Transform EffectPoint;
        public Transform CurrentEffectPoint;
        public Transform EjectionPoint;
        public int FireSoundSetID;
        public float EjectionForce = 1;
        public List<AudioSource> GunSFXSources;
        public float NonAutoCap = 1;//3 for BR55/75-Like weapon
        [Header("Pickupable Definition")]
        public Pickupable Pickup;
        public List<BoxCollider> AttachedColliders;
        [Header("Effects")]
        public float FireInterval;
        public float Recoil = 0;
        public float SingleFireRecoil = 0.5f;
        public float MaxRecoil = 2;
        public float RecoilRecoverSpeed = 5;
        public float MaxScatterAngle = 0;
        public float MaxScatterAngleAimMode = 0;
        public bool ControlledBehaviorWorkflow;
        [Header("Camera Shake")]
        public float BaseCameraShakeIntensity = 0.3f;
        public float AimModeMultiplier = 0.5f;
        public float CamShakeDecay = 5f;
        public float CamShakeSpeed = 50f;
        public float CameraShakeIntensity = 0.3f;
        public bool CreateBullet = true;
        public BipedEntity Holder;
        public Site13Event OnSingleFire = new Site13Event();
        public Site13Event OnRealProjetileFire = new Site13Event();
        public Site13Event OnOverheat = new Site13Event();
        public Site13Event OnHit = new Site13Event();
        bool FIRE0;
        public bool FIRE3 = false;
        public byte FIRE1 = 0;
        public byte FIRE2 = 0; //Semi Auto Use
        /// <summary>
        /// 
        /// </summary>
        /// <see cref="WeaponConstants"/>
        public int WeaponMode = WeaponConstants.WEAPON_MODE_NORMAL;

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
        public void Zoom()
        {
        }
        public void Unzoom()
        {

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unfire()
        {
            FIRE0 = false;
            FIRE3 = false;
            FIRE1 = 0;
        }

        private void OnEnable()
        {
            NotifyWeaponAmmo();
        }
        public void SingleFire()
        {
            OnSingleFire.Invoke();

            if ((WeaponData.CurrentMagazine > 0 && WeaponData.CurrentHeat < WeaponData.MaxHeat) || isHoldByPlayer == false)
            {
                if (FireType == WeaponFireType.FullAuto || FireType == WeaponFireType.SemiAuto)
                {

                    if (isHoldByPlayer)
                    {
                        this.WeaponData.CurrentMagazine--;
                        NotifyWeaponAmmo();
                        Holder.GetComponentInChildren<CameraShakeEffect>().SetShake(1f, true, CamShakeDecay, true, CamShakeSpeed, CameraShakeIntensity, CameraShakeIntensity);
                    }
                    //if (OnCurrentMagChanged != null)
                    //{
                    //    OnCurrentMagChanged(WeaponData.CurrentMagazine);
                    //}
                    Quaternion Rotation = this.transform.rotation;
                    Vector3 RecoilAngle = MathUtilities.RandomDirectionAngleOnXYAndZ0(Recoil / MaxRecoil * (AimingMode == 0 ? MaxScatterAngle : MaxScatterAngleAimMode), Camera.main.fieldOfView);
                    Vector3 RecoilAngle2 = MathUtilities.RandomDirectionAngleOnXYAndZ1(Recoil / MaxRecoil * (AimingMode == 0 ? MaxScatterAngle : MaxScatterAngleAimMode));
                    //Debug.Log(RecoilAngle);

                    if (WeaponAnimation != null)
                        WeaponAnimation.SetTrigger("Fire");
                    WeaponAnimation.LastTrigger = "";
                    WeaponData.CurrentHeat += WeaponData.HeatPerShot;
                    if (WeaponData.CurrentHeat > WeaponData.MaxHeat)
                    {
                        WeaponMode = WeaponConstants.WEAPON_MODE_OVERHEAT;

                        if (WeaponAnimation != null)
                            WeaponAnimation.SetTrigger("Overheat");
                    }
                    {
                        Vector3 V = Rotation.eulerAngles;
                        V += RecoilAngle;
                        Rotation = Quaternion.Euler(V);
                    }
                    if (EjectionPoint != null)
                    {
                        var GO = GameRuntime.CurrentGlobals.CurrentEffectController.Spawn(EjectionPrefab, EjectionPoint.position, EjectionPoint.rotation, Vector3.one);
                        //GO.layer = this.gameObject.layer;
                        {
                            var RIG = GO.GetComponentInChildren<Rigidbody>();
                            if (RIG != null)
                            {
                                RIG.AddForce(EjectionPoint.forward * EjectionForce, ForceMode.Impulse);
                            }
                        }
                    }
                    if (EffectPrefab != null)
                    {
                        var GO = GameRuntime.CurrentGlobals.CurrentEffectController.Spawn(EffectPrefab, CurrentEffectPoint.position, CurrentEffectPoint.rotation, Vector3.one, CurrentEffectPoint);
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
                                info.collider.attachedRigidbody.AddForce(_Rotation.normalized * WeaponData.PhysicsForce, ForceMode.Impulse);

                            }
                            //if (useEffectPointInsteadFirePoint)
                            //{
                            //    var _d = (info.point - EffectPoint.position).normalized;
                            //    Rotation = Quaternion.LookRotation(_d);
                            //}
                            {
                                var Hittable = info.collider.GetComponent<IHittable>();

                                Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, info.normal);
                                if (Hittable != null)
                                {
                                    var f = GameRuntime.CurrentGlobals.CurrentEffectController.Spawn(Hittable.HitEffectHashCode(), info.point, quaternion, Vector3.one, info.collider.transform, true);
                                    f.transform.localScale = new Vector3(1 / f.transform.lossyScale.x, 1 / f.transform.lossyScale.y, 1 / f.transform.lossyScale.z);
                                }
                                else
                                {
                                }
                                if (BulletHitEffect != null)
                                {
                                    var f = GameRuntime.CurrentGlobals.CurrentEffectController.Spawn(BulletHitEffect, info.point, quaternion, Vector3.one);
                                    f.transform.parent = info.collider.transform;
                                    //f.transform.localScale = new Vector3(1f / f.transform.lossyScale.x, 1f / f.transform.lossyScale.y, 1f / f.transform.lossyScale.z);
                                }
                                var Entity = info.collider.GetComponent<DamagableEntity>();
                                var WeakPoint = info.collider.GetComponent<WeakPoint>();
                                if (WeakPoint != null)
                                {
                                    OnHit.Invoke();
                                    WeakPoint.AttachedBioEntity.Damage(Bullet.GetPrefab().GetComponent<BaseBullet>().WeakPointDamage);
                                }
                                else if (Entity != null)
                                {
                                    CauseDamage(Entity);
                                }
                                else
                                {
                                    var Ref = info.collider.GetComponent<DamagableEntityReference>();
                                    if (Ref != null)
                                    {
                                        Entity = Ref.Reference;
                                        CauseDamage(Entity);
                                    }
                                }

                            }
                        }

                        if (Bullet != null)
                        {
                            if (CreateBullet)
                            {
                                //if (!useEffectPointInsteadFirePoint)
                                //    GameRuntime.CurrentGlobals.CurrentBulletSystem.AddBullet(BulletPrefab, FirePoint.position, Rotation, ActualHolder);
                                //else
                                {

                                    GameRuntime.CurrentGlobals.CurrentBulletSystem.AddBullet(Bullet, CurrentEffectPoint.position, Rotation, Holder.gameObject);
                                }
                            }
                        }
                    }
                    else
                    {

                        if (Bullet != null)
                        {
                            //if (!useEffectPointInsteadFirePoint)
                            //    GameRuntime.CurrentGlobals.CurrentBulletSystem.AddBullet(BulletPrefab, FirePoint.position, Rotation, ActualHolder);
                            //else
                            {
                                GameRuntime.CurrentGlobals.CurrentBulletSystem.AddBullet(Bullet, EffectPoint.position, Rotation, Holder.gameObject);
                            }
                        }
                    }
                    Recoil = Math.Min(Recoil + SingleFireRecoil, MaxRecoil);
                    if (GunSFXSources.Count > 0)
                    {
                        var source = ListOperations.ObtainOne(GunSFXSources);
                        source.Stop();
                        source.clip = SoundResources.ObtainOneClip(FireSoundSetID);
                        source.Play();
                    }
                }
            }
            else
            {
            }
        }
        private void CauseDamage(DamagableEntity Entity)
        {
            if (OnHit != null)
            {
                OnHit.Invoke();
            }
            Entity.Damage(Bullet.GetPrefab().GetComponent<BaseBullet>().BaseDamage);
        }
        float CountDown;
        float SemiCountDown;
        void __frame(float DeltaT)
        {
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
                                    SemiFireProgress(DeltaT);
                                }
                                break;
                            case WeaponFireType.Heat:
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (Recoil > 0)
                {
                    Recoil -= RecoilRecoverSpeed * DeltaT;
                }
                else if (Recoil != 0)
                {
                    Recoil = 0;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SemiFireProgress(float DeltaTime)
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
        private void Update()
        {
            if (ControlledBehaviorWorkflow) return;
            float DT = Time.deltaTime;
            __frame(DT);
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (ControlledBehaviorWorkflow)
            {
                __frame(DeltaTime);
            }
        }
        public void NotifyWeaponAmmo()
        {
            switch (AmmoDispType)
            {
                case AmmoDisp.None:
                    break;
                case AmmoDisp.TwoDig:
                    {
                        AmmoRenderers[0].material.SetFloat("_DigitNum", WeaponData.CurrentMagazine % 10);
                        AmmoRenderers[1].material.SetFloat("_DigitNum", Mathf.FloorToInt(WeaponData.CurrentMagazine / 10));
                    }
                    break;
                case AmmoDisp.ThreeDig:
                    {

                        AmmoRenderers[0].material.SetFloat("_DigitNum", WeaponData.CurrentMagazine % 10);
                        AmmoRenderers[1].material.SetFloat("_DigitNum", Mathf.FloorToInt(WeaponData.CurrentMagazine / 10) % 10);
                        AmmoRenderers[2].material.SetFloat("_DigitNum", Mathf.FloorToInt(WeaponData.CurrentMagazine / 100));
                    }
                    break;
                case AmmoDisp.Liner:
                    break;
                case AmmoDisp.Text:
                    break;
                default:
                    break;
            }
        }
    }
}
