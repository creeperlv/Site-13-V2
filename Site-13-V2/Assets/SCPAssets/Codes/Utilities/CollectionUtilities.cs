using Site13Kernel.Data;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Site13Kernel.Utilities
{
    public static class CollectionUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<T, V> ToDictionary<T, V>(List<KVPair<T, V>> RawData)
        {
            Dictionary<T, V> dic = new Dictionary<T, V>();
            foreach (var item in RawData)
            {
                dic.Add(item.Key, item.Value);
            }
            return dic;
        }
    }

}
