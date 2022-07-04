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
    public class ContinuousDamageBox : ControlledBehavior
    {
        public float DamagePerSecond=0;
        public void OnTriggerStay(Collider other)
        {
            if(DamagePerSecond != 0)
            {
                var DE = other.gameObject.GetComponentInChildren<DamagableEntity>();
                if (DE != null)
                {
                    DE.Damage(DamagePerSecond * Time.deltaTime);
                }
            }
        }
    }
}
