using Site13Kernel.Core;
using Site13Kernel.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class BagHolder : ControlledBehavior
    {
        public ControlledWeapon Weapon0 = null;
        public ControlledWeapon Weapon1 = null;
        public int CurrentWeapon;
        public ProcessedGrenade Grenade0;
        public ProcessedGrenade Grenade1;
        public int CurrentGrenade;
        public Action OnSwapWeapon;
        public Transform WeaponTransform;
        public KVList<int, int> PreDefinedEquipments = new KVList<int, int>();
        public Dictionary<int,int > Equipments=new Dictionary<int,int>();
        /// <summary>
        /// Verifies weapon slot.
        /// </summary>
        /// <returns>True if anything changed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool VerifyWeaponSlot()
        {
            if (Weapon0 == null)
            {
                if (CurrentWeapon == 0)
                {
                    CurrentWeapon = 1;
                }
            }
            if (Weapon1 == null)
            {
                if (CurrentWeapon == 1)
                {
                    CurrentWeapon = 0;
                }
            }
            return false;
        }
    }
}
