using System;
using System.Runtime.CompilerServices;

namespace Site13Kernel.Data
{
    [Serializable]
    public class PrefabReference
    {
        public bool useString;
        public string Key;
        public int ID;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PrefabReference(string key)
        {
            return new PrefabReference { useString = true, Key = key };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PrefabReference(int ID)
        {
            return new PrefabReference { useString = false, ID = ID };
        }
    }
}
