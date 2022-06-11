using CLUNL.Localization;
using Site13Kernel.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Customization
{
    [Serializable]
    public class WeaponCustomizationPalette
    {
        public string ID;
        public string TargetWeapon;
        public LocalizedString Name;
        public KVList<int, Material> MaterialMap;
        [NonSerialized]
        public Dictionary<int, Material> _MaterialMap;
    }
}
