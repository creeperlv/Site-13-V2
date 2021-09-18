using CLUNL.Data.Serializables.CheckpointSystem;
using CLUNL.Data.Serializables.CheckpointSystem.Types;
using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Effects
{
    public class BaseEffect : ControlledBehavior, ICheckpointData
    {

        public float LifeTime;
        public float TimeD;

        public string GetName()
        {
            return "";
        }

        public void Load(List<object> data)
        {
            LifeTime = (FloatNumber)data[0];
            TimeD= (FloatNumber)data[1];
        }

        public List<object> Save()
        {

            return new List<object>{LifeTime, TimeD};
        }
    }
}
