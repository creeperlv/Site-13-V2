using Site13Kernel.Core;
using System;

namespace Site13Kernel.Data
{
    [Serializable]
    public class DeathReplacement
    {
        public PrefabReference TargetPrefab;
        public DeathBodyType BodyType;
        public bool InheritOverflowDamage;
    }
}
