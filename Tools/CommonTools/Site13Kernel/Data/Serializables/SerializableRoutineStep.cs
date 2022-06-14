using Site13Kernel.Data.IO;
using System;

namespace Site13Kernel.Data.Serializables
{
    [Serializable]
    public class SerializableRoutineStep : IPureData
    {
        public SerializableLocation Location;
        public float StopRange;
        public float Speed;
    }
}
