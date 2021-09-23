using System;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Data
{
    [Serializable]
    public class PrefabDefinition
    {
        public int HashCode;
        public GameObject GameObject;
    }
    /// <summary>
    /// Help initailzes a map.
    /// </summary>
    [Serializable]
    public class PrefabList
    {
        public List<PrefabDefinition> PrefabDefinitions = new List<PrefabDefinition>();
        public Dictionary<int, GameObject> ObtainMap()
        {
            Dictionary<int, GameObject> __RESULT = new Dictionary<int, GameObject>();
            foreach (var item in PrefabDefinitions)
            {
                __RESULT.Add(item.HashCode, item.GameObject);
            }
            return __RESULT;
        }
    }
}
