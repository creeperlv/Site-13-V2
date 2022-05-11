using System;

namespace Site13Kernel.SceneBuild.Serializables
{
    [Serializable]
    public class SerializablePhysics : SerializableBase
    {
        public bool useCollider;
        public bool useRigidbody;
        public bool useGravity;
        public bool UseRenderMode;
        public float Mass;
        public float Drag;
        public float AngularDrag;
    }
}