using CLUNL.Localization;
using Site13Kernel.Data;
using System;
using System.Collections.Generic;

namespace Site13Kernel
{
    [Serializable]
    public class ArmorDescription
    {
        public string ID;
        public LocalizedString Name;
        public List<KVPair<string, PrefabReference>> ArmorPieces;
    }
}
