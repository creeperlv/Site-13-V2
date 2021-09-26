using Site13Kernel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
//using Unity.Transforms;
using UnityEngine;
namespace Site13Kernel.Utilities
{
    /// <summary>
    /// A collection of math methods.
    /// </summary>
    [Serializable]
    public struct MathUtilities
    {

        public const float PI2 = math.PI * 2;
        public const float PID2 = math.PI / 2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ObtianAngle2DUpwards(float3 v1, float3 v2)
        {
            float v = math.atan2(v2.x, v2.z) - math.atan2(v1.x, v1.z);
            return v * 180 / math.PI;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Length(float3 v1, float3 v2)
        {
            var v = v1 - v2;
            return math.sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Length(Vector3 v1, Vector3 v2)
        {
            var v = v1 - v2;
            return v.magnitude;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Length(float2 v1, float2 v2)
        {
            var v = v1 - v2;
            return math.sqrt(v.x * v.x + v.y * v.y);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Length2D(float3 v1, float3 v2)
        {
            var v = v1 - v2;
            return math.sqrt(v.x * v.x + v.z * v.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 Bezier2X(float3 P0, float3 P1, float3 Destination, float T)
        {
            var t0 = (1 - T);
            return t0 * t0 * P0 + 2 * T * t0 * P1 + T * T * Destination;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 Bezier2X(Vector3 P0, Vector3 P1, Vector3 Destination, float T)
        {
            var t0 = (1 - T);
            return t0 * t0 * P0 + 2 * T * t0 * P1 + T * T * Destination;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 GetRectangularPositionFromPolarPosition(float2 v)
        {
            return new float2(v.x * math.cos(v.y), v.x * math.sin(v.y));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 GetPolarPositionFromRectangularPosition(float2 v)
        {
            return new float2(math.sqrt(v.x * v.x + v.y * v.y), math.atan2(v.y, v.x));
        }
        /// <summary>
        /// Get rectangular position from sphere position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetRPFromSP(float3 position) => GetRPFromSP(position.x, position.y, position.z);
        /// <summary>
        /// Get rectangular position from sphere position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetRPFromSP(Vector3 position) => GetRPFromSPV3(position.x, position.y, position.z);
        /// <summary>
        /// Get rectangular position from sphere position.
        /// </summary>
        /// <param name="R"></param>
        /// <param name="Theta">Horizontal</param>
        /// <param name="Phi">Vertical</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetRPFromSP(float R, float Theta, float Phi)
        {
            float x = R * Mathf.Sin(Phi) * Mathf.Cos(Theta);
            float y = R * Mathf.Cos(Phi);
            float z = R * Mathf.Sin(Phi) * Mathf.Sin(Theta);
            return new float3(x, y, z);
        }
        /// <summary>
        /// Get rectangular position from declared sphere position.
        /// </summary>
        /// <param name="R"></param>
        /// <param name="Theta">Horizontal</param>
        /// <param name="Phi">Vertical</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetRPFromDSP(SpherePosition spherePosition)
        {
            float x = spherePosition.R * Mathf.Sin(spherePosition.φ) * Mathf.Cos(spherePosition.θ);
            float y = spherePosition.R * Mathf.Cos(spherePosition.φ);
            float z = spherePosition.R * Mathf.Sin(spherePosition.φ) * Mathf.Sin(spherePosition.θ);
            return new float3(x, y, z);
        }
        /// <summary>
        /// Get rectangular position from sphere position using Vector3.
        /// </summary>
        /// <param name="R"></param>
        /// <param name="Theta">Horizontal</param>
        /// <param name="Phi">Vertical</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetRPFromSPV3(float R, float Theta, float Phi)
        {
            float x = R * Mathf.Sin(Phi) * Mathf.Cos(Theta);
            float y = R * Mathf.Cos(Phi);
            float z = R * Mathf.Sin(Phi) * Mathf.Sin(Theta);
            return new Vector3(x, y, z);
        }
        /// <summary>
        /// Get rectangular position from declared sphere position using Vector3.
        /// </summary>
        /// <param name="R"></param>
        /// <param name="Theta">Horizontal</param>
        /// <param name="Phi">Vertical</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetRPFromDSPV3(SpherePosition spherePosition)
        {
            float x = spherePosition.R * Mathf.Sin(spherePosition.φ) * Mathf.Cos(spherePosition.θ);
            float y = spherePosition.R * Mathf.Cos(spherePosition.φ);
            float z = spherePosition.R * Mathf.Sin(spherePosition.φ) * Mathf.Sin(spherePosition.θ);
            return new Vector3(x, y, z);
        }


        /// <summary>
        /// Get sphere position from rectangular position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetSPFromRP(float3 position)
        => GetSPFromRP(position.x, position.y, position.z);

        /// <summary>
        /// Get declared sphere position from rectangular position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpherePosition GetDSPFromRP(float3 position)
        => GetDSPFromRP(position.x, position.y, position.z);

        /// <summary>
        /// Get sphere position from rectangular position using Vector3.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetSPFromRP(Vector3 position)
        => GetSPFromRPV3(position.x, position.y, position.z);
        /// <summary>
        /// Get declared sphere position from rectangular position using Vector3.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpherePosition GetDSPFromRP(Vector3 position)
        => GetDSPFromRP(position.x, position.y, position.z);

        /// <summary>
        /// Get sphere position from rectangular position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetSPFromRP(float x, float y, float z)
        {
            var R = math.sqrt(x * x + y * y + z * z);
            var Phi = math.acos(y / R);
            var Theta = math.atan2(z, x);
            return new float3(R, Theta, Phi);
        }
        /// <summary>
        /// Get sphere position from rectangular position using Vector3.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetSPFromRPV3(float x, float y, float z)
        {
            var R = math.sqrt(x * x + y * y + z * z);
            var Phi = math.acos(y / R);
            var Theta = math.atan2(z, x);
            return new Vector3(R, Theta, Phi);
        }
        /// <summary>
        /// Get declared sphere position from rectangular position using Vector3.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpherePosition GetDSPFromRP(float x, float y, float z)
        {
            var R = math.sqrt(x * x + y * y + z * z);
            var Phi = math.acos(y / R);
            var Theta = math.atan2(z, x);
            return new SpherePosition { R = R, θ = Theta, φ = Phi };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Length2DSquare(float3 v1, float3 v2)
        {
            var v = v1 - v2;
            return (v.x * v.x + v.z * v.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(float DetectValue, float DetectionOrigin, float Radius)
            => DetectValue > DetectionOrigin - Radius && DetectionOrigin < DetectionOrigin + Radius;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InRange(float V, float L, float H)
            => V > L && V < H;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 RandomDirectionAngleOnXYAndZ0(float Angle,float FOVAngle)
        {
            if (Angle == 0)
                return Vector3.zero;
            var T = Mathf.Tan(Mathf.Deg2Rad * Angle);
            float X = UnityEngine.Random.Range(-T, T);
            float Y = Mathf.Sqrt(T * T - X * X);
            Y = UnityEngine.Random.Range(-Y, Y);
            Vector3 v = new Vector3(X * FOVAngle, Y * FOVAngle, 0);
            return v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 RandomDirectionAngleOnXYAndZ1(float Angle)
        {
            if (Angle == 0)
                return Vector3.zero;
            var T = Mathf.Tan(Mathf.Deg2Rad * Angle);
            float X = UnityEngine.Random.Range(-T, T);
            float Y = Mathf.Sqrt(T * T - X * X);
            Y = UnityEngine.Random.Range(-Y, Y);
            Vector3 v = new Vector3(X, Y, 1);
            return v;
        }
    }

}