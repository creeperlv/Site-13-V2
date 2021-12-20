using Site13Kernel.Core;
using Site13Kernel.Core.Interactives;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class EventTrigger : InteractiveBase
    {
        public List<Action> Callback=new List<Action>();
        bool Executed = false;
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
    }
}
