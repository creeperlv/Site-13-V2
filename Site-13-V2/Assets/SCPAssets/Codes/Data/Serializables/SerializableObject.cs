using Site13Kernel.Core;
using Site13Kernel.Data.IO;
using Site13Kernel.Data.Serializables;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Data
{
    [Serializable]
    public class SerializableObject:IPureData
    {
        public SerializableLocation Transform;
        public SerializableBioEntity Bio;
        public SerializableDamagableEntity Entity;
        public static SerializableObject FromObject(GameObject gameObject) {
            if(gameObject == null)
            return null;
            else
            {
                SerializableObject serializableObject = new SerializableObject();
                serializableObject.Transform=Utilities.DataConversion.SerializableLocationFromGameObject(gameObject);
                {
                    var BE = gameObject.GetComponentInChildren<BioEntity>();
                    if (BE != null)
                    {
                        serializableObject.Bio = BE.ObtainData() as SerializableBioEntity;
                    }
                    else
                    {
                        serializableObject.Bio = null;
                    }
                }
                {
                    var DE=gameObject.GetComponentInChildren<DamagableEntity>();
                    if (DE != null)
                    {
                        serializableObject.Entity = DE.ObtainData() as SerializableDamagableEntity;
                    }
                    else serializableObject.Entity = null;
                }
                return serializableObject;
            }
        }
        public void ApplyToObject(GameObject gameObject) {
            DataConversion.SerializableLocationToGameObject(gameObject, Transform);
            if (Bio != null)
            {
                var BIO = gameObject.GetComponentInChildren<BioEntity>();
                if (BIO != null)
                {
                    BIO.ApplyData(Bio);
                }
            }
            if (Entity != null)
            {
                var BIO = gameObject.GetComponentInChildren<DamagableEntity>();
                if (BIO != null)
                {
                    BIO.ApplyData(Entity);
                }
            }
        }
    }
}
