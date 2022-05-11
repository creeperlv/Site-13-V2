using System;

namespace Site13Kernel.SceneBuild
{
    [Serializable]
    public class SerializableQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;
        public SerializableQuaternion(float X, float Y, float Z,float W)
        {
            x = X;
            y = Y;
            z = Z;
            w = W;
        }
        public static implicit operator SerializableQuaternion(UnityEngine.Quaternion v)
        {
            return new SerializableQuaternion(v.x, v.y, v.z,v.w);
        }
        public static implicit operator UnityEngine.Quaternion(SerializableQuaternion v)
        {
            return new UnityEngine.Quaternion(v.x, v.y, v.z,v.w);
        }
    }
}