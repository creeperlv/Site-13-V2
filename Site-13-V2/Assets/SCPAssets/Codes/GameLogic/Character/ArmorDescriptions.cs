using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.Character
{
    public class ArmorDescriptions:MonoBehaviour
    {
        public List<KVPair<string, ArmorDescription>> PrimitiveArmorDescriptions = new List<KVPair<string, ArmorDescription>>();
        public Dictionary<string, ArmorDescription> _PrimitiveArmorDescriptions = new Dictionary<string, ArmorDescription>();

        public static ArmorDescriptions Instance;
        public void Start()
        {
            Instance = this;
            _PrimitiveArmorDescriptions = CollectionUtilities.ToDictionary(PrimitiveArmorDescriptions);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArmorDescription QueryDescription(string Name)
        {
            if (Instance._PrimitiveArmorDescriptions.TryGetValue(Name, out ArmorDescription description))
            {
                return description;
            }
            else
            {
                var desc=new ArmorDescription();
                Instance._PrimitiveArmorDescriptions.Add(Name,desc);
                return desc;
            }
        }
    }
}