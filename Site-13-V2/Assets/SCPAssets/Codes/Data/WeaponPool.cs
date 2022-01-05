using Site13Kernel.Core;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, Pickupable) Instantiate(string ID, Vector3 Position, Quaternion Rotation, Transform transform)
        {
            return Instantiate(WeaponItemMap[ID].PickablePrefab, Position, Rotation, transform);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, Pickupable) Instantiate(PrefabReference gameObject, Vector3 Position, Quaternion Rotation, Transform transform)
        {
            GameObject gameObject2 = ObjectGenerator.Instantiate(gameObject, Position, Rotation, transform);
            Pickupable component = gameObject2.GetComponent<Pickupable>();

            return (gameObject2, component);
        }

    }
    [Serializable]
    public class WeaponItem
    {
        public string Name;
        public string NameFallback;
        public PrefabReference FPSPrefab;
        public GameObject NPCPrefab;
        public PrefabReference PickablePrefab;
        public Sprite WeaponIcon;
        public Material WeaponMaterial;
    }
}
