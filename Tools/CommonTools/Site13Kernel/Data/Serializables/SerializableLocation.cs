﻿using System;

namespace Site13Kernel.Data.Serializables
{
    [Serializable]
    public class SerializableLocation
    {
        public bool UseSceneLookUp;
        public string LookUpName;
        public SerializableVector3 Position;
        public SerializableQuaternion Rotation;
        public SerializableVector3 Scale;
    }
}
