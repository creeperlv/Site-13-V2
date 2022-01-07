using Site13Kernel.GameLogic.Level;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.Data
{
    [Serializable]
    public class PrefabList<T> : KVList<T, GameObject>
    {
    }
    [Serializable]
    public class PrefabReference
    {
        public bool useString;
        public string Key;
        public int ID;
        public GameObject GetPrefab()
        {
            if (useString)
            {
                return ResourceBuilder.ObtainGameObject(Key);
            }
            else return ResourceBuilder.ObtainGameObject(ID);
        }
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
    [Serializable]
    public class StringIDMappingList : KVList<string, int>
    {

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
