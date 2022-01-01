using Site13Kernel.Data.IO;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Utilities
{
    public static class ObjectGenerator
    {
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
        public static GameObject Instantiate(GameObject gameObject,int ID)
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
        public static GameObject Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation,Transform parent, string ID)
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
        public static GameObject Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation,Transform parent, int ID)
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
