using Newtonsoft.Json;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Core.Interactives;
using Site13Kernel.Data;
using Site13Kernel.Data.Serializables;
using Site13Kernel.GameLogic.AI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.GameLogic.Directors
{
    public class ScriptableDirector : ControlledBehavior
    {
        public BaseController LevelController;
        public bool isRunning = false;
        public string TargetScriptName;
        public TextAsset DefaultScript;
        static
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented,
        };
        public KVList<string, SimpleTrigger> SimpleTriggers = new KVList<string, SimpleTrigger>();
        public KVList<string, EventTrigger> EventTriggers = new KVList<string, EventTrigger>();
        public KVList<string, GameObject> ReferencedObjects = new KVList<string, GameObject>();
        public KVList<string, Transform> ReferenceLocations = new KVList<string, Transform>();
        public KVList<string, Goal> ReferenceRountines = new KVList<string, Goal>();
        public Dictionary<string, SimpleTrigger> __SimpleTriggers = new Dictionary<string, SimpleTrigger>();
        public Dictionary<string, EventTrigger> __EventTriggers = new Dictionary<string, EventTrigger>();
        public Dictionary<string, GameObject> __ReferencedObjects = new Dictionary<string, GameObject>();
        public Dictionary<string, Transform> __ReferenceLocations = new Dictionary<string, Transform>();
        public Dictionary<string, Goal> __ReferenceRountines = new Dictionary<string, Goal>();
        public Dictionary<Type, Action<EventBase>> Actions = new Dictionary<Type, Action<EventBase>>();
        public void initResources()
        {
            __SimpleTriggers = SimpleTriggers.ObtainMap();
            __EventTriggers = EventTriggers.ObtainMap();
            __ReferencedObjects = ReferencedObjects.ObtainMap();
            __ReferenceLocations = ReferenceLocations.ObtainMap();
            __ReferenceRountines = ReferenceRountines.ObtainMap();

        }
        public virtual void SetupActions()
        {

        }
        public virtual void SetupScript(List<EventBase> events)
        {

        }
        public UnityLocation FromSerializableLocation(SerializableLocation serializableLocation)
        {
            if (serializableLocation.UseSceneLookUp)
            {
                if (__ReferenceLocations.ContainsKey(serializableLocation.LookUpName))
                {
                    var t = __ReferenceLocations[serializableLocation.LookUpName];
                    return new UnityLocation { Position = t.position, Rotation = t.rotation, Scale = t.localScale };
                }
            }
            return new UnityLocation
            {
                Position = new Vector3(serializableLocation.Position.X, serializableLocation.Position.Y, serializableLocation.Position.Z),
                Rotation = new Quaternion(serializableLocation.Rotation.X, serializableLocation.Rotation.Y, serializableLocation.Rotation.Z, serializableLocation.Rotation.W),
                Scale = new Vector3(serializableLocation.Scale.X, serializableLocation.Scale.Y, serializableLocation.Scale.Z)
            };
        }
        public List<EventBase> CompileScript(string Text)
        {
            return JsonConvert.DeserializeObject<List<EventBase>>(Text, settings);
        }
    }
}
