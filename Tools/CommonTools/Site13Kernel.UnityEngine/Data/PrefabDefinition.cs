using System;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Data
{
    [Serializable]
    public class PrefabDefinition<T>
    {
        public T HashCode;
        public GameObject GameObject;
    }
    [Serializable]
    public class PrefabCollection
    {
        public static Dictionary<string, GameObject> StrPrefabs = new Dictionary<string, GameObject>();
        public static Dictionary<int, GameObject> IntPrefabs = new Dictionary<int, GameObject>();
    }
    /// <summary>
    /// Help initailzes a map.
    /// </summary>
    [Serializable]
    public class PrefabList<T>
    {
        public List<PrefabDefinition<T>> PrefabDefinitions = new List<PrefabDefinition<T>>();
        public Dictionary<T, GameObject> ObtainMap()
        {
            Dictionary<T, GameObject> __RESULT = new Dictionary<T, GameObject>();
            foreach (var item in PrefabDefinitions)
            {
                __RESULT.Add(item.HashCode, item.GameObject);
            }
            return __RESULT;
        }
    }
    [Serializable]
    public class PrefabList_StringID : PrefabList<string>
    {

    }
    [Serializable]
    public class PrefabList_IntID : PrefabList<int>
    {

    }
}
