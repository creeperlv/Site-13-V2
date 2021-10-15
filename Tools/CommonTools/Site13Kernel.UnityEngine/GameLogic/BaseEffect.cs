using CLUNL.Data.Serializables.CheckpointSystem;
using CLUNL.Data.Serializables.CheckpointSystem.Types;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Load(List<object> data)
        {
            LifeTime = (FloatNumber)data[0];
            TimeD= (FloatNumber)data[1];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public virtual List<object> Save()
        {

            return new List<object>{LifeTime, TimeD};
        }
    }
    public class SideEffectEffect : BaseEffect
    {
        public int ID;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Init()
        {
            EffectController.CurrentEffectController.Spawn(ID, this.transform.position, this.transform.rotation, this.transform.localScale, this.transform.parent);
        }
    }
}
