using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Customization
{
    public class WeaponCoatingPalettes : MonoBehaviour
    {
        public static WeaponCoatingPalettes Instance;
        public List<WeaponCustomizationPalette> Palettes;
        void Start()
        {
            Instance = this;
        }
        public static WeaponCustomizationPalette Find(string WeaponID,string CoatingID)
        {
            if (Instance == null) return null;
            foreach (var item in Instance.Palettes)
            {
                if (item.ID == CoatingID && item.TargetWeapon == WeaponID) {
                    if (item._MaterialMap == null)
                    {
                        item._MaterialMap = item.MaterialMap.ObtainMap();
                    }
                    return item;
                }
            }
            return null;
        }
    }
}
