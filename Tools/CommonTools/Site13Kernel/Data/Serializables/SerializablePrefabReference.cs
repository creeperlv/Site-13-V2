using Site13Kernel.Data.IO;
using System;

namespace Site13Kernel.Data.Serializables
{
    [Serializable]
    public class SerializablePrefabReference : IPureData
    {
        public bool useString;
        public string Key;
        public int ID;
    }
}
