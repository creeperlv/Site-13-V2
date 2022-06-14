﻿using Site13Kernel.Data.IO;
using System;
using System.Text;

namespace Site13Kernel.Data.Serializables
{
    [Serializable]
    public class SerializableVector3 : IPureData
    {
        public float X=0;
        public float Y=0;
        public float Z=0;
    }
}
