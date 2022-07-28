using Site13Kernel.Data;
using Site13Kernel.Data.Attributes;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Catalog("Player")]
    [Serializable]
    public class GiveWeaponEvent : EventBase
    {
        public Weapon weapon;
    }
}
