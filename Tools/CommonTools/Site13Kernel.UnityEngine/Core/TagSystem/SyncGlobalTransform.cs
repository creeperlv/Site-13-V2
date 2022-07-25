using Site13Kernel.Data.Serializables;

namespace Site13Kernel.Core.TagSystem
{
    public class SyncGlobalTransform:AttachableComponent
    {
        public SerializableVector3 Position;
        public SerializableQuaternion Rotation;
        public SerializableVector3 Scale;
    }
}
