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
        public string PrefabID_STR="";
        public int PrefabID_INT=-1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PID.Str", PrefabID_STR, typeof(string));
            info.AddValue("PID.Int", PrefabID_INT, typeof(int));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Load()
        {
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Save()
        {
        }
    }
}
