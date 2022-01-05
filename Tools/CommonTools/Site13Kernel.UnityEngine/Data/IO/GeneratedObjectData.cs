using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace Site13Kernel.Data.IO
{
    public class GeneratedObjectData : ControlledBehavior,IData
    {
        public PrefabReference PrefabReference;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PREFAB_REF", PrefabReference, typeof(string));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Load(IData Data)
        {
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Save()
        {
        }
    }
}
