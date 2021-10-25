using Site13Kernel.Core;
using Site13Kernel.Core.Interactives;
using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class Pickupable : InteractiveBase
    {
        public PickupItem ItemType;
        public Weapon Weapon;
        public override void Operate(float DeltaTime, float UnscaledDeltaTime, DamagableEntity Operator)
        {
            if (this.Parent != null)
            {

            }
        }
    }
    public enum PickupItem
    {
        Weapon, Grenade
    }
}
