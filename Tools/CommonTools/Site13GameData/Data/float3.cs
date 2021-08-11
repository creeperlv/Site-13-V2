using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Site13GameData.Data
{
    [Serializable]
    public struct float3 : IEquatable<float3>
    {

        public float x;

        public float y;

        public float z;

        public static readonly float3 zero;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(float3 rhs)
        {
            if (x == rhs.x && y == rhs.y)
            {
                return z == rhs.z;
            }

            return false;
        }
    }
}
