using Site13Kernel.Core;
using Site13Kernel.Core.Interactives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel
{
    public class EventTrigger : Site13Kernel.Core.Interactives.EventTrigger
    {
        //[SerializeField]
        //private List<Action> __Callback = new List<Action>();
        //public new List<Action> Callback
        //{
        //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //    get => __Callback;
        //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //    set => __Callback = value;
        //}
        public bool Executed = false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Operate(float DeltaTime, float UnscaledDeltaTime, DamagableEntity Operator)
        {
            if (!Executed)
            {
                foreach (var item in Callback)
                {
                    item();
                };
                Executed = true;
            }

        }
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public new void AddCallback(Action Callback)
        //{
        //    __Callback.Add(Callback);
        //}
    }
}
