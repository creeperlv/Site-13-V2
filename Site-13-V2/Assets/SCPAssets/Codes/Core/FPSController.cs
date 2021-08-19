using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.GameLogic;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class FPSController : ControlledBehavior
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
            Parent.OnRefresh.Add(this);
        }
        bool isRunning=false;
        public float JumpP00=1f;
        public float Gravity=9.8f;
        public float MoveFriction=9.8f;
        public ControlledWeapon Weapon0;
        public ControlledWeapon Weapon1;

        Vector3 _JUMP_V;
        Vector3 _MOVE;
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
                        _MOVE = transform.right * _V.x + transform.forward * _V.z;
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
    }
}
