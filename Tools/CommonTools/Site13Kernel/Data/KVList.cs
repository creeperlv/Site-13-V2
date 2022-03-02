using System;
using System.Collections.Generic;

namespace Site13Kernel.Data
{
    [Serializable]
    public class KVList<T, V>
    {
        public List<KVPair<T, V>> PrefabDefinitions = new List<KVPair<T, V>>();
        public Dictionary<T, V> ObtainMap()
        {
            Dictionary<T, V> __RESULT = new Dictionary<T, V>();
            foreach (var item in PrefabDefinitions)
            {
                __RESULT.Add(item.Key, item.Value);
            }
            return __RESULT;
        }
        public Dictionary<T, V> ObtainMap(Func<T, T> K_Process)
        {
            Dictionary<T, V> __RESULT = new Dictionary<T, V>();
            foreach (var item in PrefabDefinitions)
            {
                __RESULT.Add(K_Process(item.Key), item.Value);
            }
            return __RESULT;
        }
        public Dictionary<T, V> ObtainMap(Func<V, V> V_Process)
        {
            Dictionary<T, V> __RESULT = new Dictionary<T, V>();
            foreach (var item in PrefabDefinitions)
            {
                __RESULT.Add((item.Key), V_Process(item.Value));
            }
            return __RESULT;
        }
        public Dictionary<T, V> ObtainMap(Func<T, T> K_Process, Func<V, V> V_Process)
        {
            Dictionary<T, V> __RESULT = new Dictionary<T, V>();
            foreach (var item in PrefabDefinitions)
            {
                __RESULT.Add(K_Process(item.Key), V_Process(item.Value));
            }
            return __RESULT;
        }
    }
}
