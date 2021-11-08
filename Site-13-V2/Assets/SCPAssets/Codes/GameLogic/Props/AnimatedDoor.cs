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
        public string State_Close;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public override void Open()
        {
            Animator.Play(State_Open);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public override void Close()
        {
            Animator.Play(State_Close);
        }
    }
}
