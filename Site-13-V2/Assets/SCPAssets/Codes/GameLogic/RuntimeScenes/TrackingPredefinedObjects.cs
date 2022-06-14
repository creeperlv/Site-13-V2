using Site13Kernel.Data;
using Site13Kernel.Data.IO;
using Site13Kernel.Data.Serializables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.RuntimeScenes
{
    public class TrackingPredefinedObjects : MonoBehaviour,IContainsPureData
    {
        public static TrackingPredefinedObjects Instance;
        public List<GameObject> TrackingObjects = new List<GameObject>();

        public void ApplyData(IPureData data)
        {
            if(data is SerializableTrackingPredefinedObjects objects)
            {
                objects.Apply(this);
            }
        }

        public IPureData ObtainData()
        {
            return new SerializableTrackingPredefinedObjects();
        }
    }
    [Serializable]
    public class SerializableTrackingPredefinedObjects:IPureData
    {
        public List<SerializableObject> SerializableObjects;
        public static SerializableTrackingPredefinedObjects FromTrackingPredefinedObjects(TrackingPredefinedObjects origins)
        {
            SerializableTrackingPredefinedObjects ___ = new SerializableTrackingPredefinedObjects();
            foreach (var item in origins.TrackingObjects)
            {
                ___.SerializableObjects.Add(SerializableObject.FromObject(item));
            }
            return ___;
        }
        public void Apply(TrackingPredefinedObjects origins)
        {
            for (int i = 0; i < SerializableObjects.Count; i++)
            {
                //origins.TrackingObjects[i]
                if(SerializableObjects[i] != null)
                {
                    SerializableObjects[i].ApplyToObject(origins.TrackingObjects[i]);
                }
                else
                {
                    GameObject.Destroy(origins.TrackingObjects[i]);
                }
            }
        }
    }
}
