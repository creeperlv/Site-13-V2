using Site13Kernel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Data
{
    public class WeaponPool : ControlledBehavior
    {
        public static WeaponPool CurrentPool;
        public Dictionary<string, WeaponItem> WeaponItemMap = new Dictionary<string, WeaponItem>();
        public List<WeaponItem> RawWeaponItems;
        public override void Init()
        {
            CurrentPool = this;
            foreach (var item in RawWeaponItems)
            {
                WeaponItemMap.Add(item.Name, item);
            }
        }
    }
    [Serializable]
    public class WeaponItem
    {
        public string Name;
        public string NameFallback;
        public GameObject FPSPrefab;
        public GameObject NPCPrefab;
        public GameObject PickablePrefab;
        public Sprite WeaponIcon;
    }
}
