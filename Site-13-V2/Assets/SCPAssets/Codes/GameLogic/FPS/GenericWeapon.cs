using Site13Kernel.Core;
using Site13Kernel.Data;
using System;

namespace Site13Kernel.GameLogic.FPS
{
    [Serializable]
    public class GenericWeapon:ControlledBehavior
    {
        public Weapon WeaponData;
        public Site13Event OnSingleFire = new Site13Event();

    }
}
