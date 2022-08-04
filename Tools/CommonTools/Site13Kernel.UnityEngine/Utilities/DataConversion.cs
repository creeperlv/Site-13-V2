
using Site13Kernel.Data;
using Site13Kernel.Data.Serializables;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Utilities
{
    public struct DataConversion
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 QuaternionToVector4(Quaternion q)
        {
            return new Vector4(q.x, q.y, q.z, q.w);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Vector4ToQuaternion(Vector4 v)
        {
            return new Quaternion(v.x, v.y, v.z, v.w);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 DeserializeVector3(SerializableVector3 vector3)
        {
            return new Vector3(vector3.X, vector3.Y, vector3.Z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion DeserializeQuaternion(SerializableQuaternion quaternion)
        {
            return new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeserializeVector3Ref(SerializableVector3 vector3, ref Vector3 target)
        {
            target.x = vector3.X;
            target.y = vector3.Y;
            target.z = vector3.Z;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeserializeQuaternionRef(SerializableQuaternion vector3, ref Quaternion target)
        {
            target.x = vector3.X;
            target.y = vector3.Y;
            target.z = vector3.Z;
            target.w = vector3.W;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SerializableVector3 SerializeVector3(Vector3 vector)
        {
            return new SerializableVector3() { X = vector.x, Y = vector.y, Z = vector.z };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SerializableQuaternion SerializeQuaternion(Quaternion vector)
        {
            return new SerializableQuaternion() { X = vector.x, Y = vector.y, Z = vector.z, W = vector.w };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SerializableLocation SerializableLocationFromGameObject(GameObject obj)
        {
            SerializableLocation serializableLocation = new SerializableLocation();
            serializableLocation.Position = SerializeVector3(obj.transform.position);
            serializableLocation.Scale = SerializeVector3(obj.transform.localScale);
            serializableLocation.Rotation = SerializeQuaternion(obj.transform.rotation);
            return serializableLocation;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializableLocationToGameObject(GameObject obj, SerializableLocation data)
        {
            obj.transform.position = DeserializeVector3(data.Position);
            obj.transform.rotation = DeserializeQuaternion(data.Rotation);
            obj.transform.localScale = DeserializeVector3(data.Scale);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<T, V> ToDictionary<T, V>(List<KVPair<T, V>> RawData)
        {
            Dictionary<T, V> dic = new Dictionary<T, V>();
            foreach (var item in RawData)
            {
                dic.Add(item.Key, item.Value);
            }
            return dic;
        }
    }
}
