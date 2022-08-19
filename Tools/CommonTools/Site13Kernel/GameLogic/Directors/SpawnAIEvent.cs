using Site13Kernel.Data;
using Site13Kernel.Data.Serializables;
using System;
using System.Collections.Generic;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class SpawnAIEvent : EventBase
    {
        public string ID;
        public AIState State;
        public string CombatGroupID;
        public float RandomDistance = 0;
        public SerializableLocation SpawnLocation;
        public SerializableRoutine Routine;
        public List<DeathDropItem> OverrideDeathDropItems=new List<DeathDropItem>();
    }
}
