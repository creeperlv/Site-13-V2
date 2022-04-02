using Site13Kernel.Core;
using Site13Kernel.Core.Interactives;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.Effects
{
    public class KillBox : ControlledBehavior
    {
        public void OnTriggerEnter(Collider other)
        {
            var BE = other.gameObject.GetComponentInChildren<DamagableEntity>();
            if (BE != null)
                BE.Die();
        }
    }
}
