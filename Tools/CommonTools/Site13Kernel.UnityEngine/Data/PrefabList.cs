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
