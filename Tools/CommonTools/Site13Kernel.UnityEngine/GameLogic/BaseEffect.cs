using CLUNL.Data.Serializables.CheckpointSystem;
using CLUNL.Data.Serializables.CheckpointSystem.Types;
using Site13Kernel.Core;
using Site13Kernel.Core.TagSystem;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.Effects
{
    public class BaseEffect : AttachableComponent, ICheckpointData
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
}
