using CLUNL.Data.Serializables.CheckpointSystem;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.GameLogic;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class FPSController : ControlledBehavior,ICheckpointData
    {
        public float MoveSpeed=1f;
        public float RunningSpeed=1f;
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
            WalkDistance=math.PI/ 2;
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
        public float FPSCamSwingSpeed=1;
        float Pi2;
        public override void Refresh(float DeltaTime)
        {
            if (InputProcessor.CurrentInput.GetInputDown("Run"))
            {
                isRunning = true;
            }
            if (InputProcessor.CurrentInput.GetInputUp("Run"))
            {
                isRunning = false;
            }
            {
                //Move
                if (cc.isGrounded == true)
                {
                    if (InputProcessor.CurrentInput.GetInput("Jump"))
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
                        _MOVE -= _MOVE*MoveFriction * DeltaTime;
                    }
                    else
                    {

                        var _V=V.normalized*(Mathf.Sqrt(MV*MV+MH*MH));
                        if (isRunning)
                        {
                            _V *= RunningSpeed;
                        }
                        else
                        {
                            _V *= MoveSpeed;
                        }
                        _MOVE = cc.transform.right * _V.x + cc.transform.forward * _V.z;
                        WalkDistance += _MOVE.magnitude * DeltaTime* FPSCamSwingSpeed;
                        if (WalkDistance > Pi2)
                        {
                            WalkDistance = 0;
                        }
                        var LP=FPSCam.localPosition;
                        LP.x = math.cos(WalkDistance) * FPSCamSwingIntensity;
                        FPSCam.localPosition=LP;
                    }

                }
                cc.SimpleMove(_MOVE);
                

                _JUMP_V.y -= Gravity * DeltaTime;
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
            return new List<object> {transform.position,transform.rotation,Weapon0.Weapon,Weapon1.Weapon };
        }

        public void Load(List<object> data)
        {
            throw new System.NotImplementedException();
        }
    }
}
