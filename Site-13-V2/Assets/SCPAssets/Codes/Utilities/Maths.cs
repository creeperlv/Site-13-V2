using Site13Kernel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Site13Kernel.Utilities
{
    public static class Maths
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 SmoothClose(Vector3 Original, Vector3 Target, float DeltaTime)
        {
            return Original + (Target - Original) * DeltaTime;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ObtainOne<T>(List<T> data)
        {
            if (data.Count == 1) return data[0];
            return data[UnityEngine.Random.Range(0, data.Count)];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T, int) ObtainOneWithIndex<T>(List<T> data)
        {
            if (data.Count == 1) return (data[0], 0);
            var i = UnityEngine.Random.Range(0, data.Count);
            return (data[i], i);
        }
    }
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
