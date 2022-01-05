using Site13Kernel.Data;
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
        public static GameObject Instantiate(string ID, Transform Parent)
        {
            return Instantiate(ResourceBuilder.ObtainGameObject(ID), Parent, ID);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(int ID, Transform Parent)
        {
            return Instantiate(ResourceBuilder.ObtainGameObject(ID), Parent, ID);
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
        public static GameObject Instantiate(PrefabReference Ref)
        {
            if (Ref.useString)
            {
                return Instantiate(Ref.Key);

            }
            else
            {
                return Instantiate(Ref.ID);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(PrefabReference Ref, Transform Parent)
        {
            if (Ref.useString)
            {
                return Instantiate(Ref.Key, Parent);

            }
            else
            {
                return Instantiate(Ref.ID, Parent);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(PrefabReference Ref, Vector3 Position, Quaternion Rotation)
        {
            if (Ref.useString)
            {
                return Instantiate(Ref.Key, Position, Rotation);

            }
            else
            {
                return Instantiate(Ref.ID, Position, Rotation);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(PrefabReference Ref, Vector3 Position, Quaternion Rotation, Transform parent)
        {
            if (Ref.useString)
            {
                return Instantiate(Ref.Key, Position, Rotation, parent);

            }
            else
            {
                return Instantiate(Ref.ID, Position, Rotation, parent);
            }
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
            AttachID(ID, _Object);
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, int ID)
        {
            var _Object = GameObject.Instantiate(gameObject);
            AttachID(ID, _Object);
            return _Object;
        }

        private static void AttachID(int ID, GameObject _Object)
        {
            var Entity = _Object.GetComponentInChildren<GeneratedObjectData>();
            if (Entity != null)
            {
                Entity.PrefabReference = new PrefabReference { useString = false, ID = ID };
            }
            else
            {
                _Object.AddComponent<GeneratedObjectData>().PrefabReference = new PrefabReference { useString = false, ID = ID };

            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, string ID)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation);
            AttachID(ID, _Object);
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, int ID)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation);
            AttachID(ID, _Object);
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, Transform parent, string ID)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation, parent);
            var Entity = _Object.GetComponentInChildren<GeneratedObjectData>();
            AttachID(ID, _Object);
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        private static void AttachID(string ID, GameObject _Object)
        {
            var Entity = _Object.GetComponentInChildren<GeneratedObjectData>();
            if (Entity != null)
            {
                Entity.PrefabReference = new PrefabReference { useString = true, Key = ID };
            }
            else
            {
                _Object.AddComponent<GeneratedObjectData>().PrefabReference = new PrefabReference { useString = true, Key = ID };
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, Transform parent, int ID)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation, parent);
            AttachID(ID, _Object);
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Transform parent, string ID)
        {
            var _Object = GameObject.Instantiate(gameObject, parent);
            AttachID(ID, _Object);
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Transform parent, int ID)
        {
            var _Object = GameObject.Instantiate(gameObject, parent);
            AttachID(ID, _Object);
            return _Object;
        }
    }
}
