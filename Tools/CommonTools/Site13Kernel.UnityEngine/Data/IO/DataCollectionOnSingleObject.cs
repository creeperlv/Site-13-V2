using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Data.IO
{
    [Serializable]
    public class DataCollectionOnSingleObject
    {
        static Type GATOBJDATA = typeof(GeneratedObjectData);
        public GeneratedObjectData GeneratedObjectData;
        public List<IData> CollectedComponents = new List<IData>();
        public GameObject Spawn()
        {
            var __game_object=ObjectGenerator.Instantiate(GeneratedObjectData.PrefabReference);
            foreach (var item in CollectedComponents)
            {
                if (__game_object.TryGetComponent(item.GetType(), out var __component))
                {
                    if(__component is IData data)
                    {
                        data.Load(item);
                    }
                }
            }
            return __game_object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DataCollectionOnSingleObject GatherFromObject(GameObject gameObject)
        {
            DataCollectionOnSingleObject obj = new DataCollectionOnSingleObject();

            var DATA = gameObject.GetComponentInChildren<GeneratedObjectData>();
            if (DATA != null)
            {
                var datas = DATA.gameObject.GetComponents<IData>();
                obj.GeneratedObjectData = DATA;
                foreach (var item in datas)
                {
                    if (item.GetType() != GATOBJDATA)
                    {
                        obj.CollectedComponents.Add(item);
                    }
                }
                return obj;
            }
            return null;
        }
        public static implicit operator DataCollectionOnSingleObject(GameObject data)
        {
            return GatherFromObject(data);
        }
    }
}
