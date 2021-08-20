using CLUNL.Data.Serializables.CheckpointSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class BioEntity : ControlledBehavior, ICheckpointData
    {
        public string Name;

        public string ProtoTypeID;

        public float MaxHP;
        public float CurrentHP;

        public float MaxShield;
        public float CurrentShield;

        public float ShieldRecoverSpeed;
        public float HPRecoverSpeed;

        public string GetName()
        {
            return name;
        }

        public void Load(List<object> data)
        {

        }

        public List<object> Save()
        {
            return new List<object> { ProtoTypeID,
                MaxHP,
                CurrentHP,
                MaxShield,
                CurrentShield,
                ShieldRecoverSpeed,
                HPRecoverSpeed };
        }
    }
}
