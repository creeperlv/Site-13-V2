using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.FPS;
using System;
using System.Collections.Generic;

namespace Site13Kernel.GameLogic.Character
{
    [Serializable]
    public class Bag
    {
        public List<GenericWeapon> Weapons = new List<GenericWeapon>();
        public int CurrentWeapon;
        public List<GrenadeItem> Grenades = new List<GrenadeItem>();
        public int CurrentGrenade;
        public Site13Event OnUseWeapon = new Site13Event();
        public Site13Event<GenericWeapon> OnObtainWeapon = new Site13Event<GenericWeapon>();
        public Site13Event<GenericWeapon> OnDropWeapon = new Site13Event<GenericWeapon>();
        public void TryObatinWeapon(GenericWeapon GW)
        {
            foreach (var weapon in Weapons)
            {
                if (weapon.WeaponData.WeaponID == GW.WeaponData.WeaponID)
                {
                    return;
                }
            }
            OnObtainWeapon.Invoke(GW);
        }
        public void DropWeapon(GenericWeapon GW,bool RemoveFromBag=true)
        {
            OnDropWeapon.Invoke(GW);
            if (RemoveFromBag)
            {
                Weapons.Remove(GW);
                CurrentWeapon = 0;
            }
        }
        public void UseWeeapon(int Weapon)
        {
            if (CurrentWeapon == Weapon) return;
            CurrentWeapon = Weapon;
            OnUseWeapon.Invoke();
        }
    }
}
