using System;

namespace Site13Kernel.SceneBuild
{
    [Serializable]
    public class SerializableVector3
    {
        public float x;
        public float y;
        public float z;
        public SerializableVector3(float X, float Y, float Z)
        {
            x = X;
            y = Y;
            z = Z;
        }
        public static implicit operator SerializableVector3(UnityEngine.Vector3 v)
        {
            return new SerializableVector3(v.x, v.y, v.z);
        }
        public static implicit operator UnityEngine.Vector3(SerializableVector3 v)
        {
            return new UnityEngine.Vector3(v.x, v.y, v.z);
        }
    }
}