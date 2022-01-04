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
    }
}
