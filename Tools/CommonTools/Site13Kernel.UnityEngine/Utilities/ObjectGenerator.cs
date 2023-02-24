using Site13Kernel.Core;
using Site13Kernel.Core.TagSystem;
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
        public static GameObject GetPrefab(this PrefabReference pr)
        {
            if (pr.useString)
            {
                return ResourceBuilder.ObtainGameObject(pr.Key);
            }
            else return ResourceBuilder.ObtainGameObject(pr.ID);
        }
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
            ComponentHolder.TryAddHolder(_Object);
            AttachID(ID, _Object);
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, int ID)
        {
            var _Object = GameObject.Instantiate(gameObject);
            ComponentHolder.TryAddHolder(_Object);
            ComponentHolder.TryAddHolder(_Object);
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
            ComponentHolder.TryAddHolder(_Object);
            AttachID(ID, _Object);
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, int ID)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation);
            ComponentHolder.TryAddHolder(_Object);
            AttachID(ID, _Object);
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, Transform parent, string ID)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation, parent);
            if (parent != null)
                SetLayerForChildren(_Object, parent.gameObject.layer);
            ComponentHolder.TryAddHolder(_Object);
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
            if (TagSystemManager.Instance != null) TagSystemManager.Instance.AddObject(_Object);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, Transform parent, int ID)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation, parent);
            if (parent != null)
                SetLayerForChildren(_Object, parent.gameObject.layer);
            ComponentHolder.TryAddHolder(_Object);
            AttachID(ID, _Object);
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Transform parent, string ID)
        {
            var _Object = GameObject.Instantiate(gameObject, parent);
            if (parent != null)
                SetLayerForChildren(_Object, parent.gameObject.layer);
            ComponentHolder.TryAddHolder(_Object);
            AttachID(ID, _Object);
            return _Object;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLayerForChildren(GameObject obj, int Layer)
        {
            if (Layer == 0) return;
            obj.layer = Layer;
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                SetLayerForChildren(obj.transform.GetChild(i).gameObject, Layer);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLayerForChildrenWithZero(GameObject obj, int Layer)
        {
            obj.layer = Layer;
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                SetLayerForChildrenWithZero(obj.transform.GetChild(i).gameObject, Layer);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Instantiate(GameObject gameObject, Transform parent, int ID)
        {
            var _Object = GameObject.Instantiate(gameObject, parent);
            if (parent != null)
                SetLayerForChildren(_Object, parent.gameObject.layer);
            ComponentHolder.TryAddHolder(_Object);
            AttachID(ID, _Object);
            return _Object;
        }
    }
}
