using Site13Kernel.Data;
using System;
using System.Collections.Generic;

namespace Site13Kernel.SceneBuild.Serializables
{
    [Serializable]
    public class SerializableData
    {
        public PrefabReference Reference;
        public List<SerializableBase> components=new List<SerializableBase>();

    }
    public interface SerializableBase
    {

    }
}