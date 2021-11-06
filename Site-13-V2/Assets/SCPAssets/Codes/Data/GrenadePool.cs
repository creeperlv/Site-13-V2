using Site13Kernel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Data
{
    public class GrenadePool : ControlledBehavior
    {

        public static GrenadePool CurrentPool;
        public Dictionary<int, GrenadeItem> GrenadeItemMap = new Dictionary<int, GrenadeItem>();
        public List<GrenadeItem> RawGrenadeItems;
        public override void Init()
        {
            CurrentPool = this;
            foreach (var item in RawGrenadeItems)
            {
                GrenadeItemMap.Add(item.HashCode, item);
            }
        }
    }
    [Serializable]
    public class GrenadeItem
    {
        public int HashCode;
        public ProcessedGrenade Reference;
        public GameObject GamePlayPrefab;
        public GameObject PickupablePrefab;

    }
}
