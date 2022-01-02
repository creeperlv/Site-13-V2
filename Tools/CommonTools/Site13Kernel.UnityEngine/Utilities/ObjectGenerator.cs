using Site13Kernel.Data.IO;
using Site13Kernel.GameLogic.Level;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Utilities
{
    public static class ObjectGenerator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(string ID)
        {
            return Instantiate(ResourceBuilder.ObtainGameObject(ID), ID);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(int ID)
        {
            return Instantiate(ResourceBuilder.ObtainGameObject(ID), ID);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(string ID, Vector3 Position, Quaternion Rotation)
        {
            return Instantiate(ResourceBuilder.ObtainGameObject(ID), Position, Rotation, ID);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(int ID, Vector3 Position, Quaternion Rotation)
        {
            return Instantiate(ResourceBuilder.ObtainGameObject(ID), Position, Rotation, ID);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(string ID, Vector3 Position, Quaternion Rotation, Transform parent)
        {
            return Instantiate(ResourceBuilder.ObtainGameObject(ID), Position, Rotation, parent, ID);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(int ID, Vector3 Position, Quaternion Rotation, Transform parent)
        {
            return Instantiate(ResourceBuilder.ObtainGameObject(ID), Position, Rotation, parent, ID);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, string ID)
        {
            var _Object = GameObject.Instantiate(gameObject);
            var Entity = _Object.GetComponentInChildren<GeneratedObjectData>();
            if (Entity != null)
            {
                Entity.PrefabID_STR = ID;
            }
            else
            {
                _Object.AddComponent<GeneratedObjectData>().PrefabID_STR = ID;
            }
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, int ID)
        {
            var _Object = GameObject.Instantiate(gameObject);
            var Entity = _Object.GetComponentInChildren<GeneratedObjectData>();
            if (Entity != null)
            {
                Entity.PrefabID_INT = ID;
            }
            else
            {
                _Object.AddComponent<GeneratedObjectData>().PrefabID_INT = ID;
            }
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, string ID)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation);
            var Entity = _Object.GetComponentInChildren<GeneratedObjectData>();
            if (Entity != null)
            {
                Entity.PrefabID_STR = ID;
            }
            else
            {
                _Object.AddComponent<GeneratedObjectData>().PrefabID_STR = ID;
            }
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, int ID)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation);
            var Entity = _Object.GetComponentInChildren<GeneratedObjectData>();
            if (Entity != null)
            {
                Entity.PrefabID_INT = ID;
            }
            else
            {
                _Object.AddComponent<GeneratedObjectData>().PrefabID_INT = ID;
            }
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, Transform parent, string ID)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation, parent);
            var Entity = _Object.GetComponentInChildren<GeneratedObjectData>();
            if (Entity != null)
            {
                Entity.PrefabID_STR = ID;
            }
            else
            {
                _Object.AddComponent<GeneratedObjectData>().PrefabID_STR = ID;
            }
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, Transform parent, int ID)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation, parent);
            var Entity = _Object.GetComponentInChildren<GeneratedObjectData>();
            if (Entity != null)
            {
                Entity.PrefabID_INT = ID;
            }
            else
            {
                _Object.AddComponent<GeneratedObjectData>().PrefabID_INT = ID;
            }
            return _Object;
        }
    }
}
