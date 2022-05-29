using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.Character
{
    public class ArmorAssembler : ControlledBehavior
    {
        public List<KVPair<string, Transform>> ArmorPositions;
        public Dictionary<string, Transform> _ArmorPositions;
        public string UsingArmorDescription;
        public List<GameObject> TrackingArmors = new List<GameObject>();
        public void Start()
        {
            _ArmorPositions = CollectionUtilities.ToDictionary(ArmorPositions);
            Assemble();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Assemble(ArmorDescription description = null)
        {
            if (description == null)
            {
                description = ArmorDescriptions.QueryDescription(UsingArmorDescription);
            }
            RemoveAllArmorPieces();
            if (description != null)
            {
                foreach (var item in description.ArmorPieces)
                {
                    if (_ArmorPositions.TryGetValue(item.Key, out var transf))
                    {
                        var __obj = ObjectGenerator.Instantiate(item.Value, transf);
                        TrackingArmors.Add(__obj);
                    }
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAllArmorPieces()
        {
            for (int i = TrackingArmors.Count - 1; i >= 0; i--)
            {
                var item = TrackingArmors[i];
                Destroy(item);
                TrackingArmors.RemoveAt(i);
            }
        }
    }
}
