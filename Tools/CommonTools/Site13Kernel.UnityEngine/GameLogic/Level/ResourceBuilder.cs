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
        public StringIDMappingList IDMappingList;
        public Dictionary<string, GameObject> StringGameObjectMaps = new Dictionary<string, GameObject>();
        public Dictionary<int, GameObject> IntGameObjectMaps = new Dictionary<int, GameObject>();
        public Dictionary<string, int> StringIntMap = new Dictionary<string, int>();
        public List<ResourceBuilder> SubResources;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject ObtainGameObject(string ID)
        {
            ID = ID.ToUpper();
            if (Instance.StringGameObjectMaps.ContainsKey(ID))
            {
                return Instance.StringGameObjectMaps[ID];
            }
            else if (Instance.StringIntMap.ContainsKey(ID))
            {
                return Instance.IntGameObjectMaps[Instance.StringIntMap[ID]];
            }
            else return null;

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            StringGameObjectMaps = StringMaps.ObtainMap(__upper_process);
            IntGameObjectMaps = IntMaps.ObtainMap();
            StringIntMap = IDMappingList.ObtainMap(__upper_process);
            foreach (var item in SubResources)
            {
                item.__init__merge();
            }
        }
        [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
        string __upper_process(string input)
        {
            return input.ToUpper();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public void __init__merge()
        {

            DictionaryOperations.Merge(ref Instance.StringIntMap, IDMappingList.ObtainMap(__upper_process));
            DictionaryOperations.Merge(ref Instance.StringGameObjectMaps, StringMaps.ObtainMap(__upper_process));
            DictionaryOperations.Merge(ref Instance.IntGameObjectMaps, IntMaps.ObtainMap());
        }
    }
}
