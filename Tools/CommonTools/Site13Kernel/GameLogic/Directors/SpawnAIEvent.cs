using Site13Kernel.Data.Serializables;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class SpawnAIEvent : EventBase
    {
        public string ID;
        public AIState State;
        public SerializableLocation SpawnLocation;
        public SerializableRoutine Routine;
    }
}
