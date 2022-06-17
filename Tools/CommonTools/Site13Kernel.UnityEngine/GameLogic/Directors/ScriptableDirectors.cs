using Newtonsoft.Json;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Core.Interactives;
using Site13Kernel.Data;
using Site13Kernel.Data.Serializables;
using Site13Kernel.GameLogic.AI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Site13Kernel.GameLogic.Directors
{
    public class ScriptableDirector : ControlledBehavior
    {
        public BaseController LevelController;
        public bool LoadScriptOnSceneLoad = false;
        public bool isRunning = false;
        public string TargetScriptName;
        public TextAsset DefaultScript;
        public static ScriptableDirector Instance;
        static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented,
        };
        public KVList<string, SimpleTrigger> SimpleTriggers = new KVList<string, SimpleTrigger>();
        public KVList<string, EventTrigger> EventTriggers = new KVList<string, EventTrigger>();
        public KVList<string, GameObject> ReferencedObjects = new KVList<string, GameObject>();
        public KVList<string, Animator> ReferencedAnimators = new KVList<string, Animator>();
        public KVList<string, MonoBehaviour> ReferenceMonoBehaviours = new KVList<string, MonoBehaviour>();
        public KVList<string, Transform> ReferenceLocations = new KVList<string, Transform>();
        public KVList<string, Goal> ReferenceRountines = new KVList<string, Goal>();
        public Dictionary<string, SimpleTrigger> __SimpleTriggers = new Dictionary<string, SimpleTrigger>();
        public Dictionary<string, EventTrigger> __EventTriggers = new Dictionary<string, EventTrigger>();
        public Dictionary<string, GameObject> __ReferencedObjects = new Dictionary<string, GameObject>();
        public Dictionary<string, Animator> __ReferencedAnimators = new Dictionary<string, Animator>();
        public Dictionary<string, MonoBehaviour> __ReferenceMonoBehaviours = new Dictionary<string, MonoBehaviour>();
        public Dictionary<string, Transform> __ReferenceLocations = new Dictionary<string, Transform>();
        public Dictionary<string, Goal> __ReferenceRountines = new Dictionary<string, Goal>();
        public Dictionary<Type, Action<EventBase>> Actions = new Dictionary<Type, Action<EventBase>>();
        public List<PackagedEventBase> _events = new List<PackagedEventBase>();
        public void initResources()
        {
            __SimpleTriggers = SimpleTriggers.ObtainMap();
            __EventTriggers = EventTriggers.ObtainMap();
            __ReferencedObjects = ReferencedObjects.ObtainMap();
            __ReferencedAnimators = ReferencedAnimators.ObtainMap();
            __ReferenceMonoBehaviours = ReferenceMonoBehaviours.ObtainMap();
            __ReferenceLocations = ReferenceLocations.ObtainMap();
            __ReferenceRountines = ReferenceRountines.ObtainMap();
        }
        public virtual void SetupActions()
        {

        }
        public virtual void LoadScript()
        {
            var _L = JsonConvert.DeserializeObject<List<EventBase>>(DefaultScript.text, settings);
            SetupScript(_L);
        }
        public virtual void SetupScript(List<EventBase> events)
        {
            _events.Clear();
            foreach (var item in events)
            {
                var __e = new PackagedEventBase { RawEvent = item };
                if (item.UseEventTrigger)
                {
                    if (__SimpleTriggers.ContainsKey(item.EventTriggerID))
                    {
                        var t = __SimpleTriggers[item.EventTriggerID];
                        if (t != null)
                        {
                            t.AddCallback(() => ExecuteEvent(__e, __e.RawEvent.TimeDelay));
                        }
                    }
                    else
                    {
                        if (__EventTriggers.ContainsKey(item.EventTriggerID))
                        {
                            var t = __EventTriggers[item.EventTriggerID];
                            if (t != null)
                            {
                                t.AddCallback(() => ExecuteEvent(__e, __e.RawEvent.TimeDelay));
                            }
                        }
                    }
                }
                _events.Add(__e);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ExecuteEvent(PackagedEventBase e,float t=0)
        {
            //if (e) return;
            if (e.Executed && !e.RawEvent.Repeatable) return;
            {
                Debug.Log("Will Execute-1");
                e.Executed = true;
                StartCoroutine(Execute(e,t));
            }
        }
        System.Collections.IEnumerator Execute(PackagedEventBase e,float t=0)
        {
            yield return new WaitForSeconds(t);
            RealExecute(e);
        }
        void RealExecute(PackagedEventBase e)
        {
            if (Actions.TryGetValue(e.RawEvent.GetType(), out var func))
            {
                func(e.RawEvent);
            }
        }
        public void Start()
        {
            Instance = this;
            initResources();
            SetupActions();
            if (LoadScriptOnSceneLoad) LoadScript();
        }
        public void Update()
        {
            float DT = Time.deltaTime;
            float UDT = Time.unscaledDeltaTime;
            if (isRunning == false) return;
            foreach (var item in _events)
            {
                if (item.Executed && !item.RawEvent.Repeatable) continue;
                if (item.RawEvent.UseEventTrigger) continue;
                if (item.RawEvent.UseSymbolInsteadOfEventTrigger) continue;
                bool EXE;
                if (item.RawEvent.UseEventTrigger) continue;
                {
                    item.TimeD += DT;
                    if (item.TimeD > item.RawEvent.TimeDelay)
                    {
                        EXE = true;
                        if (item.RawEvent.Repeatable) item.TimeD = 0;
                    }
                    else EXE = false;
                }
                EXE = EXE & (!item.Executed || item.RawEvent.Repeatable);
                if (EXE)
                {
                    Debug.Log("Will Execute-0");
                    ExecuteEvent(item);
                }
            }
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
