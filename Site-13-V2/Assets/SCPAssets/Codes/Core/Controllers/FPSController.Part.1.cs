﻿using Site13Kernel.Core.CustomizedInput;
using System.Runtime.CompilerServices;

namespace Site13Kernel.Core.Controllers
{
    public partial class FPSController
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (FrameDelay > 0)
            {
                FrameDelay--;
                return;
            }
            Weapon = UsingWeapon == 0 ? Weapon0 : Weapon1;
            Zoom(DeltaTime);
            Movement(DeltaTime, UnscaledDeltaTime);
            FireControl(DeltaTime);
            {
                //Weapons
                Weapon.Refresh(DeltaTime, UnscaledDeltaTime);
            }
            UpdateHUD();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetState(MoveState State)
        {
            switch (State)
            {
                case MoveState.Walk:
                    FootStepSoundSource.volume = NormalFootStepVolume;
                    break;
                case MoveState.Run:
                    FootStepSoundSource.volume = RunningFootStepVolume;
                    break;
                case MoveState.Crouch:
                    FootStepSoundSource.volume = CrouchFootStepVolume;
                    break;
                default:
                    break;
            }
            MovingState = State;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(float DeltaTime)
        {
            if (InputProcessor.CurrentInput.GetInputDown("Run") && toZoom == false)
            {
                if (MovingState == MoveState.Walk) {
                    SetState(MoveState.Run);
                }
            }
            if (InputProcessor.CurrentInput.GetInputUp("Run"))
            {
                if (MovingState == MoveState.Run)
                {
                    SetState(MoveState.Walk);
                }
                //isRunning = false;
            }
            if (MovingState == MoveState.Run)
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
        }
    }
}
