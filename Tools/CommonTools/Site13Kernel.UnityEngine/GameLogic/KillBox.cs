using Site13Kernel.Core;
using UnityEngine;

namespace Site13Kernel.GameLogic.Effects
{
    public class KillBox:ControlledBehavior
    {
        public void OnTriggerEnter(Collider other)
        {
            var BE = other.gameObject.GetComponentInChildren<DamagableEntity>();
            if (BE != null)
                BE.Die();
        }
    }
}
