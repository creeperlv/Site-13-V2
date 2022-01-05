using Site13Kernel.Data.IO;
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
        public List<DataCollectionOnSingleObject> Collected;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("COLLECTED_ENTITIES", Collected, typeof(List<DataCollectionOnSingleObject>));
        }

        public override void Init()
        {
            Instance = new GenericEntityController();
        }

        public void Load(IData SavedData)
        {
            if (SavedData is GenericEntityController controller)
            {
                Collected = controller.Collected;
            }
        }

        public void Save()
        {

        }
    }
}
