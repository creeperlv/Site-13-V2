using Site13Kernel.Data.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data.Serializables
{
    public class SerializableLevelRuntimeRegistry:IPureData
    {
        public Dictionary<string, string> StringValuePool=new Dictionary<string, string>();
        public Dictionary<string, bool> BoolValuePool=new Dictionary<string, bool>();
        public Dictionary<string, float> FloatValuePool=new Dictionary<string, float>();
    }
}
