using Site13Kernel.Core;
using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class BagHolder : ControlledBehavior
    {
        public ControlledBehavior Weapon0;
        public ControlledBehavior Weapon1;
        public int CurrentWeapon;
        public ProcessedGrenade Grenade0;
        public ProcessedGrenade Grenade1;
    }
}
