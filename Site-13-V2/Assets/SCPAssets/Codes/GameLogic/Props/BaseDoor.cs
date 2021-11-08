using Site13Kernel.Core;
using Site13Kernel.Core.Interactives;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.Props
{
    public class BaseDoor : InteractiveBase
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Open()
        {

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Close()
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Operate(float DeltaTime, float UnscaledDeltaTime, DamagableEntity Operator)
        {
            Open();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void UnOperate()
        {
            Close();
        }
    }
}
