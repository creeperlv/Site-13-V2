using Site13Kernel.Data.IO;
using System;

namespace Site13Kernel.Data.Serializables
{
    [Serializable]
    public class SerializableQuaternion : IPureData
    {
        public float X;
        public float Y;
        public float Z;
        public float W;
    }
}
