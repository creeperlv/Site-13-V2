using Site13Kernel.Core;
using Site13Kernel.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class BagHolder : ControlledBehavior
    {
        public ControlledWeapon Weapon0=null;
        public ControlledWeapon Weapon1=null;
        public int CurrentWeapon;
        public ProcessedGrenade Grenade0;
        public ProcessedGrenade Grenade1;
        public Action OnSwapWeapon;
    }
}
