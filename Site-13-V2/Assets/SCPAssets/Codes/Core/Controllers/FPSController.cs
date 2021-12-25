using CLUNL.Data.Serializables.CheckpointSystem;
using CLUNL.Localization;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using Site13Kernel.GameLogic;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.UI;
using Site13Kernel.UI.Combat;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public partial class FPSController : ControlledBehavior, ICheckpointData
    {
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
        public List<AudioClip> Footsteps;
        #endregion
        #region Bio info
        public BioEntity CurrentEntity;
        #endregion
        //public ControlledWeapon Weapon0;
        //public ControlledWeapon Weapon1;
        #region FPS Viewport
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
        public GameObject Indicator;

        #endregion

        #region HUD - Status

        [Header("HUD/Status")]
        public ProgressBar HP;
        public ProgressBar Shield;

        public WeaponHUD W_HUD0;
        public WeaponHUD W_HUD1;

        public GrenadeHUD G_HUD0;
        public GrenadeHUD G_HUD1;

        public Vector2 W_HUD_PrimaryPosition;
        public Vector3 W_HUD_PrimaryScale;
        public Vector2 W_HUD_SecondaryPosition;
        public Vector3 W_HUD_SecondaryScale;


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
        bool isThrowingGrenade;
        public override void Init()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            FPSCam_BaseT = FPSCam.localPosition;
            Parent.RegisterRefresh(this);
            WalkDistance = math.PI / 2;
            FPSCamSwingIntensitySwitchDelta = FPSCamSwingRunningIntensity - FPSCamSwingIntensity;
            //if (BagHolder.Weapon0 != null)
            //{
            //    BagHolder.Weapon0.Init();

            //    BagHolder.Weapon0.Weapon.OnHit = OnHit;
            //}
            //if (BagHolder.Weapon1 != null)
            //{

            //    BagHolder.Weapon1.Init();
            //    BagHolder.Weapon1.Weapon.OnHit = OnHit;
            //}

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
                    TargetWeapon.Weapon.MeleeArea.Holder = this.gameObject;
                    TargetWeapon.Weapon.FirePoint = FirePoint;
                    TargetWeapon.Weapon.OnHit = OnHit;
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
                if (Indicator != null)
                {
                    var effect = EffectController.CurrentEffectController.Spawn(Indicator, Vector3.zero, Quaternion.identity, Vector3.one, IndicatorHolder);
                    var RT = effect.transform as RectTransform;
                    RT.anchoredPosition3D = Vector3.zero;
                    RT.localRotation = Quaternion.identity;

                }
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

                Weapon.transform.localPosition = math.lerp(Weapon.NormalPosition, Weapon.RunningPosition, WRTween);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FireControl(float DeltaTime)
        {
            if (InputProcessor.CurrentInput.GetInput("Fire"))
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
            }
            if (InputProcessor.CurrentInput.GetInputDown("Fire"))
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

            }
            if (InputProcessor.CurrentInput.GetInputDown("Combat"))
            {
                Weapon.Combat();
                if (Weapon.Weapon.WeaponMode == WeaponConstants.WEAPON_MODE_MELEE)
                {
                    CancelRun();
                }
            }
            if (InputProcessor.CurrentInput.GetInputUp("Fire"))
                Weapon.Unfire();
            if (InputProcessor.CurrentInput.GetInputDown("Reload"))
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
        public void ShowWeapon()
        {
            Weapon.ShowCoreWeaponAnimator();
            if (Weapon.CrosshairCanvasGroup != null)
            {
                Weapon.CrosshairCanvasGroup.SetActive(true);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HideWeapon()
        {
            Weapon.HideCoreWeaponAnimator();
            if (Weapon.CrosshairCanvasGroup != null)
            {
                Weapon.CrosshairCanvasGroup.SetActive(false);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CancelZoom()
        {
            toZoom = false;
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


            {
                if (InputProcessor.CurrentInput.GetInputDown("Zoom"))
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
                if (InputProcessor.CurrentInput.GetInputUp("Zoom"))
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
                        if (Camera.main.fieldOfView > Weapon.ZoomFov)
                            Camera.main.fieldOfView -= math.abs(NormalFOV - Weapon.ZoomFov) * DeltaTime * ZoomSpeed;
                    }
                    else
                    {
                        if (Weapon.ZoomHUD.alpha > 0)
                            Weapon.ZoomHUD.alpha -= DeltaTime * ZoomSpeed;
                        if (Camera.main.fieldOfView < NormalFOV)
                            Camera.main.fieldOfView += math.abs(NormalFOV - Weapon.ZoomFov) * DeltaTime * ZoomSpeed;
                    }
                }
                else
                {
                    if (InternalZoom)
                    {
                        if (Camera.main.fieldOfView > ZoomFOV)
                            Camera.main.fieldOfView -= math.abs(NormalFOV - ZoomFOV) * DeltaTime * ZoomSpeed;
                    }
                    else
                    {

                        if (Camera.main.fieldOfView < NormalFOV)
                            Camera.main.fieldOfView += math.abs(NormalFOV - ZoomFOV) * DeltaTime * ZoomSpeed;
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
                        if (Camera.main.fieldOfView > ZoomFOV)
                            Camera.main.fieldOfView -= math.abs(NormalFOV - ZoomFOV) * DeltaTime * ZoomSpeed;
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
                        if (Camera.main.fieldOfView < NormalFOV)
                            Camera.main.fieldOfView += math.abs(NormalFOV - ZoomFOV) * DeltaTime * ZoomSpeed;
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
                cc.transform.Rotate(0, InputProcessor.CurrentInput.GetAxis("MouseH") * MouseHoriztonalIntensity * DeltaTime, 0);
                var Head_V = InputProcessor.CurrentInput.GetAxis("MouseV") * MouseHoriztonalIntensity * DeltaTime;
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
        bool PrevGrounded = true;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Move(float DeltaTime)
        {

            {
                //Move
                if (cc.isGrounded == true)
                {
                    if (!PrevGrounded)
                    {

                        if (cc.isGrounded)
                        {
                            if (FootStepSoundSource != null)
                            {
                                FootStepSoundSource.Stop();
                                FootStepSoundSource.clip = Footsteps[UnityEngine.Random.Range(0, Footsteps.Count)];
                                FootStepSoundSource.Play();
                            }

                        }
                    }
                    PrevGrounded = true;
                    if (InputProcessor.CurrentInput.GetInputDown("Jump"))
                    {
                        if (cc.isGrounded)
                        {
                            if (FootStepSoundSource != null)
                            {
                                FootStepSoundSource.Stop();
                                FootStepSoundSource.clip = Footsteps[UnityEngine.Random.Range(0, Footsteps.Count)];
                                FootStepSoundSource.Play();
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
                        _JUMP_V.y = -2;
                    }
                    var MV = InputProcessor.CurrentInput.GetAxis("MoveVertical");
                    var MH = InputProcessor.CurrentInput.GetAxis("MoveHorizontal");
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
                        Weapon.Weapon.SetRecoilMax(math.clamp(Weapon.Weapon.Recoil, RunRecoil, 1f));
                    PrevGrounded = false;
                }
                if (!cc.isGrounded)
                    cc.Move(_MOVE * DeltaTime);
                else
                    cc.SimpleMove(_MOVE);
                //if (cc.velocity.magnitude != 0)
                {
                    var md = cc.velocity.magnitude * DeltaTime * FPSCamSwingSpeed;
                    if (cc.isGrounded)
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
                        if (WalkDistance > MathUtilities.PI2)
                        {
                            WalkDistance = 0;

                        }
                        if (WalkDistanceD > math.PI)
                        {
                            WalkDistanceD = 0;
                            if (FootStepSoundSource != null)
                            {
                                FootStepSoundSource.Stop();
                                FootStepSoundSource.clip = Footsteps[UnityEngine.Random.Range(0, Footsteps.Count)];
                                FootStepSoundSource.Play();
                            }
                        }
                        LP.x = math.cos(WalkDistance % MathUtilities.PI2) * CurrentFPSCamSwingIntensity;
                        LP.y = FPSCam_BaseT.y + math.abs(math.cos((WalkDistance) % MathUtilities.PI2) * CurrentFPSCamSwingIntensity * 0.5f);
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
                if (!cc.isGrounded)
                {
                    _JUMP_V.y -= Gravity * DeltaTime;

                }
                cc.Move(_JUMP_V * DeltaTime);

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
