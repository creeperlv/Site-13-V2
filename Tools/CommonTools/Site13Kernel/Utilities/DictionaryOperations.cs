using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Site13Kernel.Utilities
{
    public static class DictionaryOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Merge<T, V>(ref Dictionary<T, V> Target, Dictionary<T, V> Source)
        {
            foreach (var item in Source)
            {
                if (Target.ContainsKey(item.Key))
                {
                    Target[item.Key] = item.Value;
                }
                else
                {
                    Target.Add(item.Key, item.Value);
                }
            }
        }
    }
}
