using Site13Kernel.Core.Interactives;
using Site13Kernel.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.GameLogic.FPS
{
    public class Pickupable : InteractiveBase
    {
        public PickupItem ItemType;
        public Weapon Weapon;
    }
    public enum PickupItem
    {
        Weapon, Grenade
    }
}
