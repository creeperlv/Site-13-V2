
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Utilities
{
    public struct DataConversion
    {
        public static Vector4 QuaternionToVector4(Quaternion q)
        {
            return new Vector4(q.x, q.y, q.z, q.w);
        }
        public static Quaternion Vector4ToQuaternion(Vector4 v)
        {
            return new Quaternion(v.x, v.y, v.z, v.w);
        }
    }
}
