using Site13Kernel.Data;
using Site13Kernel.Data.IO;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    [Serializable]
    public class GenericEntityController : ControlledBehavior, IData
    {
        public static GenericEntityController Instance;
        [HideInInspector]
        public List<DataCollectionOnSingleObject> Collected = new List<DataCollectionOnSingleObject>();
        [HideInInspector]
        public List<GameObject> CollectedObjects = new List<GameObject>();
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("COLLECTED_ENTITIES", Collected, typeof(List<DataCollectionOnSingleObject>));
        }
        public GameObject SpawnObject(PrefabReference ID, Vector3 Pos, Quaternion Rot)
        {
            var __obj = ObjectGenerator.Instantiate(ID, Pos, Rot);
            CollectedObjects.Add(__obj);
            return __obj;
        }
        public GameObject SpawnObject(PrefabReference ID)
        {
            var __obj = ObjectGenerator.Instantiate(ID);
            CollectedObjects.Add(__obj);
            return __obj;
        }
        public GameObject SpawnObject(PrefabReference ID, Vector3 Pos, Quaternion Rot, Vector3 Scl)
        {
            var __obj = ObjectGenerator.Instantiate(ID, Pos, Rot);
            __obj.transform.localScale = Scl;
            CollectedObjects.Add(__obj);
            return __obj;
        }
        public override void Init()
        {
            Instance = this;
        }

        public void Load(IData SavedData)
        {
            if (SavedData is GenericEntityController controller)
            {
                Collected = controller.Collected;
                foreach (var item in Collected)
                {
                    item.Spawn();
                }
            }
        }

        public void Save()
        {
            Collected.Clear();
            foreach (var item in CollectedObjects)
            {
                if (item != null)
                {
                    Collected.Add(DataCollectionOnSingleObject.GatherFromObject(item));
                }
            }
        }
    }
}
