using Site13Kernel.Data.IO;
using System;

namespace Site13Kernel.Data
{
    [Serializable]
    public class BaseEquipment : IPureData
    {
        public int ID;
        public int MaxCount;
        public bool WillReGen;
        public float ReGenDelay;
    }
}
