using Newtonsoft.Json;
using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace Site13Kernel.SceneBuild
{
    public class SceneBuilder : MonoBehaviour
    {
        public static SceneBuilder CurrentBuilder;
        public Transform ObjectsContainer;
        public SceneDescription Description;
        public List<KVPair<string, int>> BaseMapMapping;
        void Start()
        {
            CurrentBuilder = GetComponent<SceneBuilder>();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static void NewObject(PrefabReference obj, Vector3 Position, Quaternion quaternion)
        {
            ObjectGenerator.Instantiate(obj, Position, quaternion, CurrentBuilder.ObjectsContainer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deserialize(SceneDescription description)
        {
            Description = description;
            var cC = ObjectsContainer.childCount;
            for (int i = 0; i < cC; i++)
            {
                Destroy(ObjectsContainer.GetChild(i).gameObject);
            }
            foreach (var item in Description.Objects)
            {
                var gen = ObjectGenerator.Instantiate(item.Data.Reference, ObjectsContainer);
                var eo = gen.GetComponent<EditableObject>();
                if (eo.GetComponent<EditableObject>() != null)
                {
                    item.CopyTo(eo);
                    eo.EditableData.UpdateScene();
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Serialize()
        {
            return Serialize(Description, ObjectsContainer);
        }
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static string Serialize(SceneDescription description)
        //{
        //    foreach (var item in description.Objects)
        //    {
        //        item.UpdateValue();
        //    }
        //    return JsonConvert.SerializeObject(description, settings);
        //}
        public static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            //ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Serialize(SceneDescription description, Transform Container)
        {
            if (description.Objects != null)
                description.Objects.Clear();
            else description.Objects = new List<Serializables.SerializableObject>();
            var cC = Container.childCount;
            List<EditableObject> __objs = new List<EditableObject>();
            for (int i = 0; i < cC; i++)
            {
                var eo = Container.GetChild(i).GetComponent<EditableObject>();
                if (eo != null)
                {
                    __objs.Add(eo);
                }
            }
            foreach (var item in __objs)
            {
                item.UpdateValue();
            }
            foreach (var item in __objs)
            {
                description.Objects.Add(item);
            }
            return JsonConvert.SerializeObject(description, settings);
        }
    }
}