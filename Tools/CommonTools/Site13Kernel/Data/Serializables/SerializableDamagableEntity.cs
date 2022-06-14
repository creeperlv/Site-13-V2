using Site13Kernel.Data.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data.Serializables
{
    [Serializable]
    public class SerializableDamagableEntity : IPureData
    {
        public float CurrentHP;
    }
}
