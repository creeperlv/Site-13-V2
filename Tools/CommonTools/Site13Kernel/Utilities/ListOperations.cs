using Site13Kernel.Data;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Site13Kernel.Utilities
{
    public static class ListOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ObtainOne<T>(List<T> list)
        {
            return list[RandomTool.NextInt(0,list.Count)];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> Duplicate<T>(this List<T> List)
        {
            List<T> result = new List<T>(List.Count);
            result.ElementAt(0);
            foreach (var item in List)
            {
                if(item is IDuplicatable d)
                {
                    result.Add((T)d.Duplicate());
                }else
                    result.Add(item);
            }
            return result;
        }
    }
}
