using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

namespace Site13Kernel.Core
{
    [Serializable]
    public struct SpherePosition
    {

        /// <summary>
        /// Radius
        /// </summary>
        public float R;
        /// <summary>
        /// Theta
        /// </summary>
        public float θ;
        /// <summary>
        /// Phi
        /// </summary>
        public float φ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SpherePosition(float R, float θ, float φ)
        {
            this.R = R;
            this.θ = θ;
            this.φ = φ;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SpherePosition(float3 V)
        {
            this.R = V.x;
            this.θ = V.y;
            this.φ = V.z;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SpherePosition(Vector3 V)
        {
            this.R = V.x;
            this.θ = V.y;
            this.φ = V.z;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpherePosition FromVector3(Vector3 Data)
        {
            return new SpherePosition { R = Data.x, θ = Data.y, φ = Data.z };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector3(SpherePosition sp)
        {
            return MathUtilities.GetRPFromDSPV3(sp);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator SpherePosition(Vector3 sp)
        {
            return MathUtilities.GetDSPFromRP(sp);
        }
    }
}
