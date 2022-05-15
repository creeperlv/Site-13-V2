using Site13Kernel.Data;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class GiveWeaponEvent : EventBase
    {
        public Weapon weapon;
    }
}
