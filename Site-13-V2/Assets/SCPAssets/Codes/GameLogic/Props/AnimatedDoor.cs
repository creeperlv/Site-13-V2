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
        public override void Init()
        {
            this.Parent.RegisterRefresh(this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Open()
        {
            //Animator.ResetTrigger(State_Open);
            //Animator.ResetTrigger(State_Close);
            Mode = 0;
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
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
                        Animator.Play(State_Open_Idle);
                    }
                    LastMode = 2;
                }
            }

            if (LastMode == 1)
            {
                if (CurrentD < State_Close_Length)
                {
                    CurrentD += DeltaTime;
                }
                if (CurrentD > State_Open_Length)
                {
                    if (State_Open_Idle != GameEnv.EmptyString)
                    {
                        Animator.Play(State_Close_Idle);
                    }
                    LastMode = 3;
                }
            }
            if (LastMode != Mode)
            {
                if ((LastMode == 2 && Mode == 0) || (LastMode == 3 && Mode == 1))
                {
                    return;
                }
                if (LastMode == 0 || LastMode == 1)
                    return;
                LastMode = Mode;
                switch (LastMode)
                {
                    case 0:
                        CurrentD = 0;
                        Animator.Play(State_Open);
                        break;
                    case 1:
                        CurrentD = 0;
                        Animator.Play(State_Close);
                        break;
                    default:
                        break;
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
