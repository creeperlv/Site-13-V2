using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.Core.Interactives
{
    public class EventTrigger : InteractiveBase, ITriggerable
    {
        private List<Action> __Callback = new List<Action>();
        public List<Action> Callback
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => __Callback;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => __Callback = value;
        }
        bool Executed = false;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddCallback(Action Callback)
        {
            __Callback.Add(Callback);
        }
    }
}
