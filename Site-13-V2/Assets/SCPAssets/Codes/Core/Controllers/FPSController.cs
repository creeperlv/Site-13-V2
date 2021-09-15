using CLUNL.Data.Serializables.CheckpointSystem;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.GameLogic;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class FPSController : ControlledBehavior, ICheckpointData
    {
        public float MoveSpeed = 1f;
        public float WalkRecoil = .2f;
        public float RunningSpeed = 1f;
        public float RunRecoil = .5f;
        public float RunningJumpHeight = 1f;
        public float MouseHoriztonalIntensity = 1f;
        public CharacterController cc;
        public Transform Head;
        public float MaxV;
        public float MinV;
        public override void Init()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            FPSCam_BaseT = FPSCam.localPosition;
            Parent.RegisterRefresh(this);
            WalkDistance = math.PI / 2;
            FPSCamSwingIntensitySwitchDelta = FPSCamSwingRunningIntensity - FPSCamSwingIntensity;
            if (Weapon0 != null)
                Weapon0.Init();
            if (Weapon1 != null)
                Weapon1.Init();
        }
        Vector3 FPSCam_BaseT;
        bool isRunning = false;
        public float JumpP00 = 1f;
        public float Gravity = 9.8f;
        public float MoveFriction = 9.8f;
        public ControlledWeapon Weapon0;
        public ControlledWeapon Weapon1;
        public Transform FPSCam;
        Vector3 _JUMP_V;
        Vector3 _MOVE;
        public float Cycle;
        float WalkDistance;
        public float FPSCamSwingIntensity = 0.1f;
        public float FPSCamSwingRunningIntensity = 0.1f;
        public float FPSCamSwingIntensitySwitchSpeed = 1f;
        public float FPSCamSwingSpeed = 1;
        public float WalkIncreasementIntensity = 1;
        public float RunIncreasementIntensity = 0.5f;
        public int FrameDelay = 1;
        public int UsingWeapon = 0;
        [HideInInspector]
        public float CurrentFPSCamSwingIntensity = 0f;
        [HideInInspector]
        public float WRTween = 0;
        float FPSCamSwingIntensitySwitchDelta;
        [Header("Zoom")]
        public float NormalFOV = 9.8f;
        public float ZoomFOV;
        public float ZoomSpeed;
        public CanvasGroup ZoomHUD;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SwapWeapon()
        {
            UseWeapon(UsingWeapon == 0 ? 1 : 0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UseWeapon(int i)
        {
            UsingWeapon = i;
        }
        /// <summary>
        /// Switch Walk/Running.s
        /// </summary>
        /// <param name="DeltaTime"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyWR(float DeltaTime)
        {
            //Debug.Log(WRTween);
            Weapon.transform.localPosition = math.lerp(Weapon.NormalPosition, Weapon.RunningPosition, WRTween);
            //Weapon.transform.localRotation = DataConversion.Vector4ToQuaternion(
            //    math.lerp(
            //        DataConversion.QuaternionToVector4(Weapon.NormalRotation),
            //        DataConversion.QuaternionToVector4(Weapon.RunningRotation),
            //        WRTween
            //        )
            //    ); 
            Weapon.transform.localRotation = Quaternion.Euler(
                math.lerp(
                    Weapon.NormalRotationEuler,
                    Weapon.RunningRotationEuler,
                    WRTween
                    )
                );
        }
        bool isWalking = true;
        ControlledWeapon Weapon;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (FrameDelay > 0)
            {
                FrameDelay--;
                return;
            }
            Weapon = UsingWeapon == 0 ? Weapon0 : Weapon1;
            if (InputProcessor.CurrentInput.GetInputDown("Run") && toZoom == false)
            {
                isRunning = true;
            }
            if (InputProcessor.CurrentInput.GetInputUp("Run"))
            {
                isRunning = false;
            }
            if (isRunning == true)
            {
                if (WRTween < 1)
                {
                    isWalking = false;
                    WRTween += DeltaTime * FPSCamSwingIntensitySwitchSpeed;
                    ApplyWR(DeltaTime);
                }
                else
                {
                    if (WRTween != 1)
                    {
                        WRTween = 1;
                        ApplyWR(DeltaTime);
                    }
                }
            }
            else
            {
                if (WRTween > 0)
                {
                    WRTween -= DeltaTime * FPSCamSwingIntensitySwitchSpeed;
                    ApplyWR(DeltaTime);
                }
                else
                {
                    if (WRTween != 0)
                    {
                        WRTween = 0;
                        ApplyWR(DeltaTime);
                        isWalking = true;
                    }
                }

            }
            Zoom(DeltaTime);
            Move(DeltaTime);
            Rotation(DeltaTime);
            FireControl(DeltaTime);
            {
                //Weapons
                Weapon.Refresh(DeltaTime, UnscaledDeltaTime);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FireControl(float DeltaTime)
        {
            if (InputProcessor.CurrentInput.GetInput("Fire"))
            {
                if (isWalking)
                    Weapon.Fire();
                else Weapon.Unfire();
            }
            //if (InputProcessor.CurrentInput.GetInputDown("Fire"))
            //{
            //}
            if (InputProcessor.CurrentInput.GetInputUp("Fire"))
                Weapon.Unfire();
        }
        bool toZoom = false;
        bool InternalZoom = false;
        bool WeaponZooom = false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ShowWeapon()
        {
            Weapon.ShowCoreWeaponAnimator();

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HideWeapon()
        {
            Weapon.HideCoreWeaponAnimator();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Zoom(float DeltaTime)
        {
            {
                if (InputProcessor.CurrentInput.GetInputDown("Zoom"))
                {
                    isRunning = false;
                    toZoom = true;
                    Weapon.Weapon.CurrentEffectPoint = Weapon.ZoomEffectPoint;
                    HideWeapon();
                }
                if (InputProcessor.CurrentInput.GetInputUp("Zoom"))
                {
                    toZoom = false;
                    Weapon.Weapon.CurrentEffectPoint = Weapon.Weapon.EffectPoint;
                    ShowWeapon();
                }
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
            if (Weapon != null)
            {
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
            }
            else
            {
                {
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Move(float DeltaTime)
        {

            {
                //Move
                if (cc.isGrounded == true)
                {
                    if (InputProcessor.CurrentInput.GetInputDown("Jump"))
                    {
                        if (isRunning)
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
                        if (isRunning)
                        {
                            Weapon.Weapon.Recoil = (math.clamp(Weapon.Weapon.Recoil, RunRecoil, 1f));
                            _MOVE *= RunningSpeed;
                        }
                        else
                        {
                            Weapon.Weapon.Recoil = (math.clamp(Weapon.Weapon.Recoil, WalkRecoil, 1f));
                            _MOVE *= MoveSpeed;
                        }
                    }

                }
                else
                {
                    Weapon.Weapon.Recoil = (math.clamp(Weapon.Weapon.Recoil, RunRecoil, 1f));
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

                        WalkDistance += md * (isRunning ? RunIncreasementIntensity : WalkIncreasementIntensity);

                        //if (WalkDistance > Pi2)
                        //{
                        //    WalkDistance = 0;
                        //}
                        var LP = FPSCam.localPosition;
                        //if (md != 0)
                        {
                            if (isRunning)
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
            return new List<object> { transform.position, transform.rotation, Weapon0.Weapon, Weapon1.Weapon };
        }

        public void Load(List<object> data)
        {
            throw new System.NotImplementedException();
        }
    }
}
