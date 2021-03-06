using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.Props
{
    public class AnimatedDoor : BaseDoor
    {
        public Animator Animator;
        public string State_Open;
        public string State_Open_Idle;
        public float State_Open_Length;
        float CurrentD;
        public string State_Close;
        public float State_Close_Length;
        public string State_Close_Idle;
        public int Mode = 0;
        public int LastMode = 3;
        public bool IsWorking;
        public AudioSource OpenSFXSource;
        public AudioSource CloseSFXSource;

        public override void Init()
        {
            this.Parent.RegisterRefresh(this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Open()
        {
            Mode = 0;
        }
        string LastState;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void OnFrame(float DeltaTime, float UnscaledDeltaTime)
        {
            if (GameRuntime.CurrentGlobals.isPaused) return;
            if (LastMode == 0)
            {
                if (CurrentD < State_Open_Length)
                {
                    CurrentD += DeltaTime;
                }
                if (CurrentD > State_Open_Length)
                {
                    if (State_Open_Idle != GameEnv.EmptyString)
                    {
                        if (LastState != State_Open_Idle)
                        {
                            Animator.Play(State_Open_Idle);
                            LastState = State_Open_Idle;
                        }
                    }
                    IsWorking = false;
                    //LastMode = 2;
                }
            }
            else
            if (LastMode == 1)
            {
                if (CurrentD < State_Close_Length)
                {
                    CurrentD += DeltaTime;
                }
                if (CurrentD > State_Close_Length)
                {
                    if (State_Open_Idle != GameEnv.EmptyString)
                    {
                        if (LastState != State_Close_Idle)
                        {
                            Animator.Play(State_Close_Idle);
                            LastState = State_Close_Idle;
                        }
                    }
                    //LastMode = 3;
                    IsWorking = false;
                }
            }
            if (LastMode != Mode)
            {
                if (IsWorking)
                {
                    //Wait till anime done;
                }
                else
                {
                    LastMode = Mode;
                    switch (LastMode)
                    {
                        case 0:
                            CurrentD = 0;
                            if (OpenSFXSource != null)
                            {
                                OpenSFXSource.Play();
                            }
                            if (LastState != State_Open)
                            {
                                Animator.Play(State_Open);
                                LastState = State_Open;
                            }
                            break;
                        case 1:
                            CurrentD = 0;
                            if (CloseSFXSource != null)
                            {
                                CloseSFXSource.Play();
                            }
                            if (LastState != State_Close)
                            {
                                Animator.Play(State_Close);
                                LastState = State_Close;
                            }
                            break;
                        default:
                            break;
                    }
                    IsWorking = true;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Close()
        {
            //Animator.ResetTrigger(State_Open);
            //Animator.ResetTrigger(State_Close);
            Mode = 1;
        }
    }
}
