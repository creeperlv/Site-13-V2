using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    [Serializable]
    public class TaggedGenericEntityController : GenericEntityController
    {
        public string Tag;
        public static Dictionary<string, TaggedGenericEntityController> Controllers = new Dictionary<string, TaggedGenericEntityController>();
        public override void Init()
        {
            if (TaggedGenericEntityController.Controllers.ContainsKey(Tag))
            {
                Controllers[Tag] = this;
            }
            else
            {
                Controllers.Add(Tag, this);
            }
        }
        public static GameObject SpawnObject(string tag, PrefabReference ID, Vector3 Pos, Quaternion Rot)
        {
            if (Controllers.ContainsKey(tag))
            {
                var __obj = ObjectGenerator.Instantiate(ID, Pos, Rot);
                ComponentHolder.AddComponent(__obj, new EntityTag { Tag = tag });
                Controllers[tag].CollectedObjects.Add(__obj);
                return __obj;

            }
            else return null;
        }
        public static GameObject SpawnObject(string tag, PrefabReference ID)
        {
            if (Controllers.ContainsKey(tag))
            {
                var __obj = ObjectGenerator.Instantiate(ID);
                ComponentHolder.AddComponent(__obj, new EntityTag { Tag = tag });
                Controllers[tag].CollectedObjects.Add(__obj);
                return __obj;

            }
            else return null;
        }
        public static GameObject SpawnObject(string tag, PrefabReference ID, Vector3 Pos, Quaternion Rot, Vector3 Scl)
        {
            if (Controllers.ContainsKey(tag))
            {
                var __obj = ObjectGenerator.Instantiate(ID, Pos, Rot);
                ComponentHolder.AddComponent(__obj,new EntityTag { Tag = tag });
                __obj.transform.localScale = Scl;
                Controllers[tag].CollectedObjects.Add(__obj);
                return __obj;

            }
            else return null;
        }
    }
}
