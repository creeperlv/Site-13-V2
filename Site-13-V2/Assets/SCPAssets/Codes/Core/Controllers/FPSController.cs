using CLUNL.Data.Serializables.CheckpointSystem;
using CLUNL.Localization;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using Site13Kernel.GameLogic;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.GameLogic.Level;
using Site13Kernel.UI;
using Site13Kernel.UI.Combat;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.Core.Controllers
{
    public partial class FPSController : ControlledBehavior, ICheckpointData
    {
        public static FPSController Instance = null;
        #region Movement and recoil by move
        [Header("Move")]
        public float MoveSpeed = 1f;
        public float WalkRecoil = .2f;
        public float RunningSpeed = 1f;
        public float CrouchSpeed = 1f;
        public SimulatedRigidBodyOverCharacterController SRBoCC;
        public float RunRecoil = .5f;
        public float RunningJumpHeight = 1f;
        public float MouseHoriztonalIntensity = 1f;
        [Header("Move/CharacterController and Collider")]
        public CharacterController cc;
        public CapsuleCollider CurrentCollider;
        public float CrouchHeight;
        public float NormalHeight;
        public float HeightExchangeSpeed = 1;
        public Transform Head;
        public float MaxV;
        public float MinV;
        Vector3 FPSCam_BaseT;
        MoveState MovingState = MoveState.Walk;
        public float JumpP00 = 1f;
        public float Gravity = 9.8f;
        public float MoveFriction = 9.8f;
        public bool isMoveLocked = false;
        public bool isRotateLocked = false;
        public float NormalFootStepVolume = 0.5f;
        public float RunningFootStepVolume = 0.75f;
        public float CrouchFootStepVolume = 0.2f;
        public AudioSource FootStepSoundSource;
        public KVList<int, List<AudioClip>> Footsteps;
        public KVList<int, float> FootstepVolumeMultipliers;
        public Dictionary<int, float> _FootstepVolumeMultipliers;
        float VolumeMultiplier = 1;
        public Dictionary<int, List<AudioClip>> _FootSteps;
        public int CurrentStandingMaterial;
        [Header("Floating Movement")]
        public Vector3 FloatPositionDelta = new Vector3(0, -0.025f, 0);
        public float ToNormalSpeed = 5;
        public float ToFloatSpeed = 3;
        #endregion
        #region Bio info
        public BioEntity CurrentEntity;
        #endregion
        //public ControlledWeapon Weapon0;
        //public ControlledWeapon Weapon1;
        #region FPS Viewport
        public Camera MainCam;
        public Transform FPSCam;
        public Transform RealMainCam;
        Vector3 _JUMP_V;
        Vector3 _MOVE;
        public float Cycle;
        float WalkDistance;
        float WalkDistanceD;
        public Vector3 NormalHeadPosition;
        public Vector3 CrouchHeadPosition;
        public float HeadExchangeSpeed;
        public float WalkDistanceMultiplier = 1;
        public float FPSCamSwingIntensity = 0.1f;
        public float FPSCamSwingRunningIntensity = 0.1f;
        public float FPSCamSwingCrouchIntensity = 0.1f;
        public float FPSCamSwingIntensitySwitchSpeed = 1f;
        public float FPSCamSwingSpeed = 1;
        public float WalkIncreasementIntensity = 1;
        public float CrouchIncreasementIntensity = 1;
        public float RunIncreasementIntensity = 0.5f;
        public int FrameDelay = 1;
        //public int UsingWeapon = 0;
        [HideInInspector]
        public float CurrentFPSCamSwingIntensity = 0f;
        [HideInInspector]
        public float WRTween = 0;
        float FPSCamSwingIntensitySwitchDelta;
        #endregion
        #region HUD - Zoom

        [Header("HUD/Zoom")]
        public float NormalFOV = 9.8f;
        public float ZoomFOV;
        public float ZoomSpeed;
        public CanvasGroup ZoomHUD;

        #endregion

        #region HUD - Combat

        public Transform IndicatorHolder;
        public PrefabReference Indicator;

        #endregion

        #region HUD - Status

        [Header("HUD/Status")]
        public ProgressBar HP;
        public List<ProgressBar> Shield;

        public WeaponHUD W_HUD0;
        public WeaponHUD W_HUD1;

        public GrenadeHUD G_HUD0;
        public GrenadeHUD G_HUD1;

        public Vector2 W_HUD_PrimaryPosition;
        public Vector3 W_HUD_PrimaryScale;
        public Vector2 W_HUD_SecondaryPosition;
        public Vector3 W_HUD_SecondaryScale;

        public Image E_HUD_ICON;
        public Text E_HUD_COUNT;

        #endregion

        #region HUD - Hints
        [Header("HUD/Hints")]
        public PropertiedText InteractHint;
        #endregion
        [Header("Interact")]
        #region Interact
        public float Reach = 2;
        public float SightDistance = 15;
        public float InteractSensitivity = 0.2f;
        #endregion
        [Header("Generic Animation")]
        public bool AnimatedBodyEnabled;
        public GameObject Body;
        public Animator BodyAnimator;
        public string MOVEMENT_TRIGGER;
        public float Intensity;
        public float MaxFinalIntensity;
        public int FRAMEIGNORANCE = 2;
        [Header("Weapon")]
        public BagHolder BagHolder;
        public Transform FirePoint;
        public Transform ZoomEffectPoint;
        [Header("Grenades")]
        public Animator GrenadeThrower;
        public List<HoldGrenade> Grenades;
        public Transform Grenade_ThrowOutPoint;
        public float GrenadeThrowForce;
        public float GrenadeThrowTime;
        public float GrenadeThrowAnimationTime;
        public float GrenadeThrowD;
        public bool Grenade_Throwed = false;
        public bool Grenade_Throwing = false;
        [Header("Equipments")]
        public int SelectedEquipment;
        public int LastSelectedEquipment=-1;
        public KVList<int, GameLogic.Equipments.EquipmentBase> Equipments = new KVList<int, GameLogic.Equipments.EquipmentBase>();
        public Dictionary<int, GameLogic.Equipments.EquipmentBase> __equipments = new Dictionary<int, GameLogic.Equipments.EquipmentBase>();
        [Header("UX")]
        public List<GameObject> ShieldDownObject = new List<GameObject>();

        [Header("Flash Light")]
        public bool FlashLightEnabled;
        public GameObject FlashLightObject;
        [Header("Watch Info")]
        public GameObject WatchLayer;
        bool isThrowingGrenade;
        bool Interrupt00 = false;
        Vector3 _WalkPosition;
        Vector3 _RunPosition;
        public override void Init()
        {

            Instance = this;
            __equipments = Equipments.ObtainMap();
            CurrentEntity.OnShieldDown = () =>
            {
                foreach (var item in ShieldDownObject)
                {
                    if (!item.activeSelf) item.SetActive(true);
                }
            };
            CurrentEntity.OnDie = () =>
            {
                Parent.UnregisterRefresh(this);
                {
                    try
                    {
                        if (CurrentEntity.DeathReplacements.Count > 0)
                        {
                            foreach (var item in CurrentEntity.DeathReplacements)
                            {
                                DeathBodyGen(item);
                            }
                        }
                        else
                        {
                            DeathBodyGen(new DeathReplacement { TargetPrefab = new PrefabReference { ID = CurrentEntity.DeathBodyReplacementID }, BodyType = CurrentEntity.deathBodyType });
                        }
                    }
                    catch (Exception e)
                    {
                        Debugger.CurrentDebugger.Log(e);
                    }
                    if (CurrentEntity.Controller != null)
                        CurrentEntity.Controller.DestroyEntity(CurrentEntity);
                    else
                    {
                        if (CurrentEntity.ControlledObject != null)
                        {
                            Destroy(CurrentEntity.ControlledObject.gameObject);
                        }
                        else
                            Destroy(CurrentEntity.gameObject);
                    }
                }
                if (OnDeath != null)
                {
                    OnDeath();
                }
                return true;
            };
            _FootSteps = Footsteps.ObtainMap();
            _FootstepVolumeMultipliers = FootstepVolumeMultipliers.ObtainMap();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            FPSCam_BaseT = FPSCam.localPosition;
            //Parent.RegisterRefresh(this);
            WalkDistance = math.PI / 2;
            FPSCamSwingIntensitySwitchDelta = FPSCamSwingRunningIntensity - FPSCamSwingIntensity;
            G_HUD0.holder = BagHolder;
            G_HUD1.holder = BagHolder;

            BagHolder.OnSwapWeapon = () =>
            {
                ControlledWeapon TargetWeapon = null;
                WeaponHUD TargetHUD = null;

                if (BagHolder.Weapon0 != null)
                {
                    TargetWeapon = BagHolder.Weapon0;
                    TargetHUD = W_HUD0;
                    TargetHUD.ListeningWeapon = TargetWeapon;
                    if (BagHolder.CurrentWeapon == 0) goto APPLY;
                }
                if (BagHolder.Weapon1 != null)
                {
                    TargetWeapon = BagHolder.Weapon1;
                    TargetHUD = W_HUD1;
                    TargetHUD.ListeningWeapon = TargetWeapon;
                    if (BagHolder.CurrentWeapon == 1) goto APPLY;
                }
            APPLY:
                if (TargetWeapon != null)
                {

                    TargetWeapon.Init();
                    if (TargetWeapon.ZoomEffectPoint == null)
                        TargetWeapon.ZoomEffectPoint = ZoomEffectPoint;
                    if (TargetWeapon.Weapon.MeleeArea != null)
                        TargetWeapon.Weapon.MeleeArea.Holder = this.gameObject;
                    TargetWeapon.Weapon.FirePoint = FirePoint;
                    TargetWeapon.Weapon.OnHit = OnHit;
                    TargetWeapon.Weapon.isHoldByPlayer = true;
                    TargetWeapon.Weapon.ActualHolder = this.gameObject;
                    TargetHUD.isPercentage = TargetWeapon.isPercentage;
                    if (WeaponPool.CurrentPool.WeaponItemMap.ContainsKey(TargetWeapon.Weapon.Base.WeaponID))
                    {
                        TargetHUD.DisplayTextTitle.text = Language.Find(TargetWeapon.Weapon.Base.WeaponID + ".DispName", WeaponPool.CurrentPool.WeaponItemMap[TargetWeapon.Weapon.Base.WeaponID].NameFallback);
                        TargetHUD.IconImg.sprite = WeaponPool.CurrentPool.WeaponItemMap[TargetWeapon.Weapon.Base.WeaponID].WeaponIcon;
                        if (WeaponPool.CurrentPool.WeaponItemMap[TargetWeapon.Weapon.Base.WeaponID].WeaponMaterial != null)
                            TargetHUD.IconImg.material = WeaponPool.CurrentPool.WeaponItemMap[TargetWeapon.Weapon.Base.WeaponID].WeaponMaterial;

                    }
                    else
                    {
                        Debugger.CurrentDebugger.LogError($"{TargetWeapon.Weapon.Base.WeaponID} does not exists in the weapon map!");
                    }
                }
                UseWeapon(BagHolder.CurrentWeapon);
            };
            BagHolder.OnSwapWeapon();

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnHit()
        {
            if (IndicatorHolder != null)
            {
                {
                    var PREFAB = ResourceBuilder.ObtainGameObject(Indicator.ID);
                    if (PREFAB != null)
                    {
                        var effect = EffectController.CurrentEffectController.Spawn(Indicator, Vector3.zero, Quaternion.identity, Vector3.one, IndicatorHolder);
                        var RT = effect.transform as RectTransform;
                        RT.anchoredPosition3D = Vector3.zero;
                        RT.localRotation = Quaternion.identity;

                    }
                    else
                    {
                    }
                }
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
                        if (CurrentEntity.Controller != null && DeathBodyReplacementID != -1)
                        {
                            CurrentEntity.Controller.Instantiate(DeathBodyReplacementID, this.transform.position, this.transform.rotation, CurrentEntity.ControlledObject.transform.parent);

                        }
                        else
                        {
                            ObjectGenerator.Instantiate(DR.TargetPrefab, this.transform.position, this.transform.rotation, CurrentEntity.ControlledObject.transform.parent);

                        }
                    }
                    break;
                case DeathBodyType.Effect:
                    {
                        EffectController.CurrentEffectController.Spawn(DR.TargetPrefab, this.transform.position, this.transform.rotation);
                    }
                    break;
                case DeathBodyType.Regular:
                    {
                        ObjectGenerator.Instantiate(DR.TargetPrefab, this.transform.position, this.transform.rotation, CurrentEntity.ControlledObject.transform.parent);
                    }
                    break;
                case DeathBodyType.Explosion:
                    {
                        GameObject effect;
                        effect = EffectController.CurrentEffectController.Spawn(DR.TargetPrefab, this.transform.position, this.transform.rotation);
                        effect.GetComponent<ExplosionEffect>().Explode();
                    }
                    break;
                default:
                    break;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SwapWeapon()
        {
            UseWeapon(BagHolder.CurrentWeapon == 0 ? 1 : 0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UseWeapon(int i)
        {
            BagHolder.CurrentWeapon = i;
            ApplySwitchWeapon();
        }
        /// <summary>
        /// Switch Walk/Running.s
        /// </summary>
        /// <param name="DeltaTime"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyWR(float DeltaTime)
        {
            if (Weapon != null)
            {

                Weapon.transform.localPosition = math.lerp(_WalkPosition, _RunPosition, WRTween);
                Weapon.transform.localRotation = Quaternion.Euler(
                    math.lerp(
                        Weapon.NormalRotationEuler,
                        Weapon.RunningRotationEuler,
                        WRTween
                        )
                    );
            }
        }
        bool isWalking = true;
        ControlledWeapon Weapon;
        bool FireControl0 = false;
        bool FireControl1 = false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FireControl(float DeltaTime)
        {
            if (FoundationStatus)
                return;
            if (InputProcessor.GetAxis("Fire") > 0.5f)
            {
                if (isWalking)
                {
                    if (Weapon.CanZoom == false && InternalZoom == true)
                    {

                    }
                    else
                        Weapon.Fire();
                }
                else Weapon.Unfire();

                if (FireControl0 == false)
                {

                    if (isWalking)
                    {
                        if (Weapon.CanZoom == false && InternalZoom == true)
                        {

                        }
                        else
                            Weapon.Press();
                    }
                    else Weapon.Unfire();
                    FireControl0 = true;
                }
                FireControl1 = false;
            }
            else

            {
                if (FireControl1 == false)
                {
                    Weapon.Unfire();
                    FireControl1 = true;
                    FireControl0 = false;
                }
            }
            if (InputProcessor.GetInputDown("Combat"))
            {
                Weapon.Combat();
                if (Weapon.Weapon.WeaponMode == WeaponConstants.WEAPON_MODE_MELEE)
                {
                    CancelRun();
                }
            }
            if (InputProcessor.GetInputDown("Reload"))
            {
                if (Weapon.Weapon.CanReload())
                {
                    CancelZoom();
                }
                Weapon.Reload();
                if (Weapon.Weapon.WeaponMode == WeaponConstants.WEAPON_MODE_RELOAD_STAGE_1 || Weapon.Weapon.WeaponMode == WeaponConstants.WEAPON_MODE_RELOAD_STAGE_0)
                {
                    CancelRun();
                }
            }
        }
        bool toZoom = false;
        bool InternalZoom = false;
        bool WeaponZooom = false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ShowWeapon(bool UseTakeOut= false)
        {
            Weapon.ShowCoreWeaponAnimator(UseTakeOut);
            if (Weapon.CrosshairCanvasGroup != null)
            {
                Weapon.CrosshairCanvasGroup.SetActive(true);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HideWeapon(bool IsZooming = true)
        {
            Weapon.HideCoreWeaponAnimator();

            if (Weapon.CrosshairCanvasGroup != null)
            {
                if (IsZooming)
                {
                    if (!Weapon.CanZoom)
                        Weapon.CrosshairCanvasGroup.SetActive(false);
                }
                else
                    Weapon.CrosshairCanvasGroup.SetActive(false);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CancelZoom()
        {
            toZoom = false;
            if (!FoundationStatus)
                if (Weapon != null)
                {
                    Weapon.Weapon.CurrentEffectPoint = Weapon.Weapon.EffectPoint;
                    Weapon.Weapon.AimingMode = 0;
                    ShowWeapon();
                }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Zoom(float DeltaTime)
        {
            if (FoundationStatus) return;
            {
                if (InputProcessor.GetInputDown("Zoom"))
                {
                    if (Weapon != null)
                    {
                        if (Weapon.Weapon.WeaponMode == WeaponConstants.WEAPON_MODE_RELOAD_STAGE_0 || Weapon.Weapon.WeaponMode == WeaponConstants.WEAPON_MODE_RELOAD_STAGE_1)
                        {
                            return;
                        }
                    }
                    CancelRun();
                    toZoom = true;
                    if (Weapon != null)
                    {
                        if (!Weapon.CanZoom)
                        {
                            Weapon.Unfire();
                        }
                        Weapon.Weapon.CurrentEffectPoint = Weapon.ZoomEffectPoint;
                        Weapon.Weapon.AimingMode = 1;
                        HideWeapon();

                    }
                }
                if (InputProcessor.GetInputUp("Zoom"))
                {
                    CancelZoom();
                }
            }
            if (Weapon != null)
            {
                if (Weapon.Weapon.WeaponMode == WeaponConstants.WEAPON_MODE_RELOAD_STAGE_1 || Weapon.Weapon.WeaponMode == WeaponConstants.WEAPON_MODE_RELOAD_STAGE_0)
                {
                    toZoom = false;
                }
                if (Weapon.CanZoom)
                {
                    //ZoomHUD
                    InternalZoom = false;
                    WeaponZooom = toZoom;
                }
                else
                {
                    InternalZoom = toZoom;
                }
                if (Weapon.CanZoom)
                {
                    if (WeaponZooom)
                    {
                        if (Weapon.ZoomHUD.alpha < 1)
                            Weapon.ZoomHUD.alpha += DeltaTime * ZoomSpeed;
                        if (Weapon.ZoomInEffect != null)
                        {
                            if (Weapon.ZoomHUD.alpha > 0.5f)
                            {
                                if (Weapon.ZoomInEffectState == 0)
                                {
                                    Weapon.ZoomInEffectState = 1;
                                    Weapon.ZoomInEffect.Play();
                                }
                            }
                            else
                            {
                                Weapon.ZoomInEffectState = 0;
                            }
                        }
                        {
                            if (!Weapon.ZoomHUD.gameObject.activeSelf)
                            {
                                Weapon.ZoomHUD.gameObject.SetActive(true);
                                Weapon.Weapon.CameraShakeIntensity = Weapon.Weapon.BaseCameraShakeIntensity * Weapon.Weapon.AimModeMultiplier;
                            }
                        }
                        if (MainCam.fieldOfView > Weapon.ZoomFov)
                            MainCam.fieldOfView -= math.abs(NormalFOV - Weapon.ZoomFov) * DeltaTime * ZoomSpeed;
                    }
                    else
                    {
                        if (Weapon.ZoomHUD.alpha > 0)
                            Weapon.ZoomHUD.alpha -= DeltaTime * ZoomSpeed;
                        else
                        {
                            if (Weapon.ZoomHUD.gameObject.activeSelf)
                            {
                                Weapon.ZoomHUD.gameObject.SetActive(false);
                                Weapon.Weapon.CameraShakeIntensity = Weapon.Weapon.BaseCameraShakeIntensity;
                            }
                        }
                        if (Weapon.ZoomInEffect != null)
                        {
                            if (Weapon.ZoomHUD.alpha < 0.5f)
                            {
                                if (Weapon.ZoomOutEffectState == 0)
                                {
                                    Weapon.ZoomOutEffectState = 1;
                                    Weapon.ZoomOutEffect.Play();

                                }
                            }
                            else
                            {
                                Weapon.ZoomOutEffectState = 0;
                            }
                        }
                        if (MainCam.fieldOfView < NormalFOV)
                            MainCam.fieldOfView += math.abs(NormalFOV - Weapon.ZoomFov) * DeltaTime * ZoomSpeed;
                    }
                }
                else
                {
                    if (InternalZoom)
                    {
                        if (MainCam.fieldOfView > ZoomFOV)
                            MainCam.fieldOfView -= math.abs(NormalFOV - ZoomFOV) * DeltaTime * ZoomSpeed;
                    }
                    else
                    {

                        if (MainCam.fieldOfView < NormalFOV)
                            MainCam.fieldOfView += math.abs(NormalFOV - ZoomFOV) * DeltaTime * ZoomSpeed;
                    }
                }
            }
            else
            {
                {
                    InternalZoom = toZoom;
                    if (InternalZoom)
                    {
                        if (Weapon != null)
                        {
                            if (Weapon.HUDCanvas != null)
                            {
                                if (Weapon.HUDCanvas.activeSelf)
                                {
                                    Weapon.HUDCanvas.SetActive(false);
                                }
                            }
                        }
                        if (MainCam.fieldOfView > ZoomFOV)
                            MainCam.fieldOfView -= math.abs(NormalFOV - ZoomFOV) * DeltaTime * ZoomSpeed;
                    }
                    else
                    {
                        if (Weapon != null)
                        {
                            if (Weapon.HUDCanvas != null)
                            {
                                if (!Weapon.HUDCanvas.activeSelf)
                                {
                                    Weapon.HUDCanvas.SetActive(true);
                                }
                            }
                        }
                        if (MainCam.fieldOfView < NormalFOV)
                            MainCam.fieldOfView += math.abs(NormalFOV - ZoomFOV) * DeltaTime * ZoomSpeed;
                    }
                }

            }
            {
                if (InternalZoom)
                {
                    if (ZoomHUD.alpha < 1)
                        ZoomHUD.alpha += DeltaTime * ZoomSpeed;
                }
                else
                {
                    if (ZoomHUD.alpha > 0)
                        ZoomHUD.alpha -= DeltaTime * ZoomSpeed;
                }
            }

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotation(float DeltaTime)
        {
            {
                //View rotation
                cc.transform.Rotate(0, InputProcessor.GetAxis("MouseH") * MouseHoriztonalIntensity * DeltaTime * Data.Settings.CurrentSettings.MouseSensibly, 0);
                var Head_V = InputProcessor.GetAxis("MouseV") * MouseHoriztonalIntensity * DeltaTime * Data.Settings.CurrentSettings.MouseSensibly;
                var ea = Head.localEulerAngles;
                ea.x += Head_V;
                if (ea.x < 180)
                {
                    ea.x = Mathf.Clamp(ea.x, MinV, MaxV);
                }
                else
                {
                    ea.x = Mathf.Clamp(ea.x, 360 + MinV, 360);

                }
                Head.localEulerAngles = ea;
            }

        }
        [Header("MissionHint")]
        public GameObject MissionHint_HintObject;
        public Text MissionHint_TextHolder;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IssueMission(string Text)
        {
            MissionHint_TextHolder.text = Text;
            StartCoroutine(ShowMission());
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator ShowMission()
        {
            MissionHint_HintObject.SetActive(false);
            yield return null;
            MissionHint_HintObject.SetActive(true);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool isGrounded()
        {
            //return Physics.Raycast(cc.transform.position, Vector3.down, out _, cc.height / 2 + 0.05f, GameRuntime.CurrentGlobals.LayerExcludePlayerAndAirBlockAndEventTrigger);
            return cc.isGrounded;
        }
        bool PrevGrounded = true;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Move(float DeltaTime)
        {
            {
                var cc_isG = isGrounded();
                //Move
                if (cc_isG == true)
                {
                    if (!PrevGrounded)
                    {
                        if (cc_isG)
                        {
                            if (FootStepSoundSource != null)
                            {
                                PlayFootstep();
                            }

                        }
                    }
                    if (Weapon != null)
                    {
                        _WalkPosition = Maths.SmoothClose(_WalkPosition, Weapon.NormalPosition, DeltaTime * ToNormalSpeed);
                        _RunPosition = Maths.SmoothClose(_RunPosition, Weapon.RunningPosition, DeltaTime * ToNormalSpeed);
                    }
                    PrevGrounded = true;
                    if (InputProcessor.GetInputDown("Jump"))
                    {
                        if (cc_isG)
                        {
                            if (FootStepSoundSource != null)
                            {
                                PlayFootstep();
                            }

                        }
                        if (MovingState == MoveState.Run)
                        {
                            _JUMP_V.y = Mathf.Sqrt(RunningJumpHeight * Gravity * 2);
                        }
                        else
                        {

                            _JUMP_V.y = Mathf.Sqrt(JumpP00 * Gravity * 2);
                        }

                    }
                    else
                    {
                        _JUMP_V.y = -Gravity;
                    }
                    var MV = InputProcessor.GetAxis("MoveVertical");
                    var MH = InputProcessor.GetAxis("MoveHorizontal");
                    var V = new Vector3(MH, 0, MV);
                    if (MV == 0 && MH == 0)
                    {
                        _MOVE -= _MOVE * MoveFriction * DeltaTime;
                        if (_MOVE.magnitude <= 0.03f)
                        {
                            _MOVE = Vector3.zero;
                        }
                    }
                    else
                    {
                        _MOVE = cc.transform.right * (MH * math.sqrt(1 - (MV * MV) * .5f)) + cc.transform.forward * (MV * math.sqrt(1 - (MH * MH) * .5f));
                        if (MovingState == MoveState.Run)
                        {
                            if (Weapon != null)
                                Weapon.Weapon.SetRecoilMax(math.clamp(Weapon.Weapon.Recoil, RunRecoil, 1f));
                            _MOVE *= RunningSpeed;
                        }
                        else if (MovingState == MoveState.Crouch)
                        {
                            //Weapon.Weapon.SetRecoilMax(math.clamp(Weapon.Weapon.Recoil, RunRecoil, 1f));
                            _MOVE *= CrouchSpeed;
                        }
                        else
                        {
                            if (Weapon != null)
                                Weapon.Weapon.SetRecoilMax(math.clamp(Weapon.Weapon.Recoil, WalkRecoil, 1f));
                            _MOVE *= MoveSpeed;
                        }
                    }

                }
                else
                {
                    if (Weapon != null)
                    {
                        Weapon.Weapon.SetRecoilMax(math.clamp(Weapon.Weapon.Recoil, RunRecoil, 1f));
                        if (Weapon != null)
                        {
                            _WalkPosition = Maths.SmoothClose(_WalkPosition, Weapon.NormalPosition + FloatPositionDelta, DeltaTime * ToFloatSpeed);
                            _RunPosition = Maths.SmoothClose(_RunPosition, Weapon.RunningPosition + FloatPositionDelta, DeltaTime * ToFloatSpeed);
                        }
                    }
                    PrevGrounded = false;
                }
                if (!cc_isG)
                    cc.Move(_MOVE * DeltaTime);
                else
                    cc.SimpleMove(_MOVE);
                //if (cc.velocity.magnitude != 0)
                {
                    var md = cc.velocity.magnitude * DeltaTime * FPSCamSwingSpeed;
                    if (cc_isG)
                    {
                        switch (MovingState)
                        {
                            case MoveState.Walk:
                                WalkDistance += md * WalkIncreasementIntensity;
                                WalkDistanceD += md * WalkIncreasementIntensity;
                                break;
                            case MoveState.Run:
                                WalkDistance += md * RunIncreasementIntensity;
                                WalkDistanceD += md * RunIncreasementIntensity;
                                break;
                            case MoveState.Crouch:
                                WalkDistance += md * CrouchIncreasementIntensity;
                                WalkDistanceD += md * CrouchIncreasementIntensity;
                                break;
                            default:
                                break;
                        }


                        //if (WalkDistance > Pi2)
                        //{
                        //    WalkDistance = 0;
                        //}
                        var LP = FPSCam.localPosition;
                        //if (md != 0)
                        {
                            if (MovingState == MoveState.Run)
                            {
                                if (CurrentFPSCamSwingIntensity < FPSCamSwingRunningIntensity)
                                {
                                    CurrentFPSCamSwingIntensity += DeltaTime * FPSCamSwingIntensitySwitchDelta * FPSCamSwingIntensitySwitchSpeed;
                                }
                                else
                                {
                                    CurrentFPSCamSwingIntensity = FPSCamSwingRunningIntensity;
                                }
                            }
                            else if (MovingState == MoveState.Crouch)
                            {
                                if (CurrentFPSCamSwingIntensity < FPSCamSwingCrouchIntensity)
                                {
                                    CurrentFPSCamSwingIntensity += DeltaTime * FPSCamSwingIntensitySwitchDelta * FPSCamSwingIntensitySwitchSpeed;
                                }
                                else
                                {
                                    CurrentFPSCamSwingIntensity = FPSCamSwingCrouchIntensity;
                                }
                            }
                            else
                            {
                                if (CurrentFPSCamSwingIntensity > FPSCamSwingIntensity)
                                {
                                    CurrentFPSCamSwingIntensity -= DeltaTime * FPSCamSwingIntensitySwitchDelta * FPSCamSwingIntensitySwitchSpeed;
                                }
                                else
                                {
                                    CurrentFPSCamSwingIntensity = FPSCamSwingIntensity;
                                }
                            }
                        }
                        if (WalkDistance * WalkDistanceMultiplier > MathUtilities.PI2)
                        {
                            WalkDistance = 0;
                        }
                        if (WalkDistanceD * WalkDistanceMultiplier > math.PI)
                        {
                            WalkDistanceD = 0;
                            PlayFootstep();
                        }
                        LP.x = math.cos(WalkDistance * WalkDistanceMultiplier % MathUtilities.PI2) * CurrentFPSCamSwingIntensity;
                        LP.y = FPSCam_BaseT.y + math.abs(math.cos((WalkDistance * WalkDistanceMultiplier) % MathUtilities.PI2) * CurrentFPSCamSwingIntensity * 0.5f);
                        FPSCam.localPosition = LP;
                    }
                }
                if (cc.velocity.x == 0)
                {
                    //_MOVE -= new Vector3(_MOVE.x * MoveFriction * DeltaTime, 0, 0);
                    _MOVE.x = 0;
                }
                if (cc.velocity.z == 0)
                {
                    //_MOVE -= new Vector3(0, 0, _MOVE.z * MoveFriction * DeltaTime);
                    _MOVE.z = 0;
                }
                if (!cc_isG)
                {
                    _JUMP_V.y -= Gravity * DeltaTime;

                }
                cc.Move(_JUMP_V * DeltaTime);

            }
        }
        public void PlayFootstep()
        {
            if (Physics.Raycast(MainCam.transform.position, Vector3.down, out RaycastHit hit, 3f, GameRuntime.CurrentGlobals.LayerExcludePlayerAndAirBlock, QueryTriggerInteraction.Collide))
            {
                var SM = hit.collider.GetComponent<StandMaterial>();
                if (SM != null)
                {
                    CurrentStandingMaterial = SM.MaterialID;
                    VolumeMultiplier = _FootstepVolumeMultipliers[CurrentStandingMaterial];
                }
            }
            if (FootStepSoundSource != null)
            {
                FootStepSoundSource.Stop();
                var L = _FootSteps[CurrentStandingMaterial];
                FootStepSoundSource.clip = L[UnityEngine.Random.Range(0, L.Count)];
                FootStepSoundSource.Play();
            }
        }
        public override void FixedRefresh(float DeltaTime, float UnscaledDeltaTime)
        {
        }

        public string GetName()
        {
            return "Player-0";
        }

        public List<object> Save()
        {
            return new List<object> { transform.position, transform.rotation, BagHolder.Weapon0.Weapon, BagHolder.Weapon0.Weapon };
        }

        public void Load(List<object> data)
        {
            throw new System.NotImplementedException();
        }
    }
}
