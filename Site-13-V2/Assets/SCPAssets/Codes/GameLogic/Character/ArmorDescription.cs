using CLUNL.Localization;
using Site13Kernel.Data;
using System;
using System.Collections.Generic;

namespace Site13Kernel.GameLogic.Character
{
    [Serializable]
    public class ArmorDescription
    {
        public string ID;
        public LocalizedString Name;
        public List<KVPair<string, PrefabReference>> ArmorPieces = new List<KVPair<string, PrefabReference>>();
        public void RemoveArmorPiece(string Name)
        {
            for (int i = ArmorPieces.Count - 1; i >= 0; i--)
            {
                var item = ArmorPieces[i];
                if (item.Key == Name) ArmorPieces.RemoveAt(i);
            }
        }
        public void PutArmorPiece(string Name, PrefabReference prefab)
        {
            foreach (var item in ArmorPieces)
            {
                if (item.Key == Name)
                {
                    item.Value = prefab;
                    return;
                }
            }
            ArmorPieces.Add(new KVPair<string, PrefabReference>() { Key = Name, Value = prefab });
        }
    }
}