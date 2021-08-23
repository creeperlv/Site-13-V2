using CLUNL.Data.Serializables.CheckpointSystem;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.GameLogic;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class FPSController : ControlledBehavior, ICheckpointData
    {
        public float MoveSpeed=1f;
        public float WalkRecoil=.2f;
        public float RunningSpeed=1f;
        public float RunRecoil=.5f;
        public float RunningJumpHeight=1f;
        public float MouseHoriztonalIntensity=1f;
        public CharacterController cc;
        public Transform Head;
        public float MaxV;
        public float MinV;
        public override void Init()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Parent.RegisterRefresh(this);
            Pi2 = math.PI * 2;
            WalkDistance = math.PI / 2;
            FPSCamSwingIntensitySwitchDelta = FPSCamSwingRunningIntensity - FPSCamSwingIntensity;
            if (Weapon0 != null)
                Weapon0.Init();
            if (Weapon1 != null)
                Weapon1.Init();
        }
        bool isRunning=false;
        public float JumpP00=1f;
        public float Gravity=9.8f;
        public float MoveFriction=9.8f;
        public ControlledWeapon Weapon0;
        public ControlledWeapon Weapon1;
        public Transform FPSCam;
        Vector3 _JUMP_V;
        Vector3 _MOVE;
        public float Cycle;
        float WalkDistance;
        public float FPSCamSwingIntensity=0.1f;
        public float FPSCamSwingRunningIntensity=0.1f;
        public float FPSCamSwingIntensitySwitchSpeed=1f;
        public float FPSCamSwingSpeed=1;
        float Pi2;
        public int FrameDelay=1;
        public int UsingWeapon=0;
        [HideInInspector]
        public float CurrentFPSCamSwingIntensity=0f;
        [HideInInspector]
        public float WRTween=0;
        float FPSCamSwingIntensitySwitchDelta;
        public void SwapWeapon()
        {
            UseWeapon(UsingWeapon == 0 ? 1 : 0);
        }
        public void UseWeapon(int i)
        {
            UsingWeapon = i;
        }
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
        ControlledWeapon Weapon;
        public override void Refresh(float DeltaTime)
        {
            if (FrameDelay > 0)
            {
                FrameDelay--;
                return;
            }
            Weapon = UsingWeapon == 0 ? Weapon0 : Weapon1;
            if (InputProcessor.CurrentInput.GetInputDown("Run"))
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
                    }
                }

            }
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
                    var MV=InputProcessor.CurrentInput.GetAxis("MoveVertical");
                    var MH=InputProcessor.CurrentInput.GetAxis("MoveHorizontal");
                    var V=new Vector3(MH,0,MV);
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

                        //var _V=V.normalized*(Mathf.Sqrt(MV*MV+MH*MH)/Sqrt2);
                        //if (isRunning)
                        //{
                        //    _V *= RunningSpeed;
                        //}
                        //else
                        //{
                        //    _V *= MoveSpeed;
                        //}
                        //_MOVE = cc.transform.right * _V.x + cc.transform.forward * _V.z;
                        _MOVE = cc.transform.right * (MH * math.sqrt(1 - (MV * MV) * .5f)) + cc.transform.forward * (MV * math.sqrt(1 - (MH * MH) * .5f));
                        if (isRunning)
                        {
                            Weapon.Recoil = (math.clamp(Weapon.Recoil, RunRecoil, 1f));
                            _MOVE *= RunningSpeed;
                        }
                        else
                        {
                            Weapon.Recoil = (math.clamp(Weapon.Recoil, WalkRecoil, 1f));
                            _MOVE *= MoveSpeed;
                        }
                        //if (cc.velocity.magnitude != 0)
                        //{
                        //}
                        //WalkDistance += _V.magnitude * DeltaTime * FPSCamSwingSpeed;
                    }

                }
                else
                {
                    Weapon.Recoil = (math.clamp(Weapon.Recoil, RunRecoil, 1f));
                }
                if (!cc.isGrounded)
                    cc.Move(_MOVE * DeltaTime);
                else
                    cc.SimpleMove(_MOVE);
                //if (cc.velocity.magnitude != 0)
                {
                    var md=cc.velocity.magnitude * DeltaTime * FPSCamSwingSpeed;
                    if (cc.isGrounded)
                    {

                        WalkDistance += md;

                        if (WalkDistance > Pi2)
                        {
                            WalkDistance = 0;
                        }
                        var LP=FPSCam.localPosition;
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
                        LP.x = math.cos(WalkDistance) * CurrentFPSCamSwingIntensity;
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
            {
                //View rotation
                cc.transform.Rotate(0, InputProcessor.CurrentInput.GetAxis("MouseH") * MouseHoriztonalIntensity * DeltaTime, 0);
                var Head_V=InputProcessor.CurrentInput.GetAxis("MouseV") * MouseHoriztonalIntensity * DeltaTime;
                var ea=Head.localEulerAngles;
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
            {
                //Weapons
                Weapon.Refresh(DeltaTime);
            }
        }
        public override void FixedRefresh(float DeltaTime)
        {
        }

        public string GetName()
        {
            throw new System.NotImplementedException();
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
