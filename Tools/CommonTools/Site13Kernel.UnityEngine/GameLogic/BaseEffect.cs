using CLUNL.Data.Serializables.CheckpointSystem;
using CLUNL.Data.Serializables.CheckpointSystem.Types;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Effects
{
    public class BaseEffect : ControlledBehavior, ICheckpointData
    {

        public float LifeTime;
        public float TimeD;

        public virtual string GetName()
        {
            return "";
        }

        public virtual void Load(List<object> data)
        {
            LifeTime = (FloatNumber)data[0];
            TimeD= (FloatNumber)data[1];
        }

        public virtual List<object> Save()
        {

            return new List<object>{LifeTime, TimeD};
        }
    }
    public class SideEffectEffect : BaseEffect
    {
        public int ID;
        public override void Init()
        {
            EffectController.CurrentEffectController.Spawn(ID, this.transform.position, this.transform.rotation, this.transform.localScale, this.transform.parent);
        }
    }
}
