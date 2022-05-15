using Site13Kernel.Data.Serializables;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class SpawnPlayerEvent : EventBase
    {
        public string PlayerID;
        public SerializableLocation Location;
    }
}
