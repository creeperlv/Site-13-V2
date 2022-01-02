using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Site13Kernel.GameLogic.Level
{
    public class ResourceBuilder : ControlledBehavior
    {
        public static ResourceBuilder Instance;
        public bool isMain = false;
        public bool isControlled = false;
        public bool isSub = false;
        public PrefabList<string> StringMaps;
        public PrefabList<int> IntMaps;
        public Dictionary<string, GameObject> StringGameObjectMaps;
        public Dictionary<int, GameObject> IntGameObjectMaps;
        public List<ResourceBuilder> SubResources; [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject ObtainGameObject(string ID)
        {
            return Instance.StringGameObjectMaps[ID];
        }
        public static GameObject ObtainGameObject(int ID)
        {
            return Instance.IntGameObjectMaps[ID];
        }
        public void Start()
        {
            if (!isControlled)
            {
                if (isMain)
                {
                    __init();
                }
                if (isSub)
                {
                    __init__merge();
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Init()
        {
            if (isControlled)
            {
                if (isMain)
                {
                    __init();
                }
                if (isSub)
                {
                    __init__merge();
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void __init()
        {

            Instance = this;
            StringGameObjectMaps = StringMaps.ObtainMap();
            IntGameObjectMaps = IntMaps.ObtainMap();
            foreach (var item in SubResources)
            {
                item.__init__merge();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public void __init__merge()
        {

            DictionaryOperations.Merge(ref StringGameObjectMaps, StringMaps.ObtainMap());
            DictionaryOperations.Merge(ref Instance.IntGameObjectMaps, IntMaps.ObtainMap());
        }
    }
}
