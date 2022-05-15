using System;

namespace Site13Kernel.Data.Serializables
{
    [Serializable]
    public class SerializableRoutineStep
    {
        public SerializableLocation Location;
        public float StopRange;
        public float Speed;
    }
}
