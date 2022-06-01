using System;
using System.Runtime.CompilerServices;

namespace Site13Kernel.GameLogic.RuntimeScenes
{
    [Serializable]
    public class LRTRegistryItem
    {
        public string Name;
        public RegistryItemDataType DataType;
        public string DefaultString;
        public float DefaultFloat;
        public bool DefaultBoolean;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object ObatinValue()
        {
            switch (DataType)
            {
                case RegistryItemDataType.STRING:
                    return LevelRuntimeRegistry.QueryString(Name, DefaultString);
                case RegistryItemDataType.FLOAT:
                    return LevelRuntimeRegistry.QueryFloat(Name, DefaultFloat);
                case RegistryItemDataType.BOOL:
                    return LevelRuntimeRegistry.QueryBool(Name,DefaultBoolean);
                default:
                    return null;
            }
        }
        public override string ToString()
        {
            var v = ObatinValue();
            if (v != null) return v.ToString();
            else return "";
        }
    }
}
