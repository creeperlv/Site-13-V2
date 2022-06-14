using Site13Kernel.Data.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data.Serializables
{
    [Serializable]
    public class SerializableBioEntity : IPureData
    {
        public int CombatRelationGroup;

        public float MaxShield;
        public float CurrentShield;

        public float ShieldRecoverSpeed;
        public float HPRecoverSpeed;

        public float ShieldRecoverDelay;
        public float HPRecoverDelay;

        public float ShieldRecoverCountDown;
        public float HPRecoverCountDown;
    }
}
