using System.Runtime.CompilerServices;
//using Unity.Transforms;
using UnityEngine;
namespace Site13Kernel.Utilities
{
    public static class MathExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 MUL(this Vector3 L, Vector3 R)
        {
            return new Vector3(L.x * R.x, L.y * R.y, L.z * R.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 DVI(this Vector3 L, Vector3 R)
        {
            return new Vector3(L.x / R.x, L.y / R.y, L.z / R.z);
        }
    }
}