using System;

namespace Site13Kernel.SceneBuild.Serializables
{
    [Serializable]
    public class SerializableTransform : SerializableBase
    {
        public SerializableVector3 Position;
        public SerializableQuaternion Rotation;
        public SerializableVector3 Scale;
    }
}