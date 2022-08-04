using Newtonsoft.Json;
using Site13Kernel.Data.IO;
using Site13Kernel.Data.Serializables;
using Site13Kernel.Data;
using System.Collections.Generic;
using UnityEngine;
using Site13Kernel.Utilities;
using Site13Kernel.Core;

namespace Site13Kernel.GameLogic.RuntimeScenes
{
    public class LevelRuntimeRegistry : MonoBehaviour, IContainsPureData
    {
        public static LevelRuntimeRegistry Instance;
        [JsonIgnore]
        public List<KVPair<string, string>> PredefinedStringValues;
        [JsonIgnore]
        public List<KVPair<string, bool>> PredefinedBoolValues;
        [JsonIgnore]
        public List<KVPair<string, float>> PredefinedFloatValues;

        public Site13Event<KVPair<string, bool>> BoolValueWatcher = new Site13Event<KVPair<string, bool>>();
        public Site13Event<KVPair<string, string>> StringValueWatcher = new Site13Event<KVPair<string, string>>();
        public Site13Event<KVPair<string, float>> FloatValueWatcher = new Site13Event<KVPair<string, float>>();
        public void Start()
        {
            Instance = this;
            __Serializable.StringValuePool = DataConversion.ToDictionary(PredefinedStringValues);
            __Serializable.BoolValuePool = DataConversion.ToDictionary(PredefinedBoolValues);
            __Serializable.FloatValuePool = DataConversion.ToDictionary(PredefinedFloatValues);
        }
        public static string QueryString(string Name, string Fallback = null)
        {
            if (Instance != null)
            {
                if (Instance.__Serializable.StringValuePool.TryGetValue(Name, out string Value))
                {
                    return Value;
                }
                else return Fallback;
            }
            else return Fallback;
        }
        public static bool QueryBool(string Name, bool Fallback = false)
        {
            if (Instance != null)
            {
                if (Instance.__Serializable.BoolValuePool.TryGetValue(Name, out var value))
                {
                    return value;
                }
                else return Fallback;
            }
            else return Fallback;
        }
        public static float QueryFloat(string Name, float Fallback = 0.0f)
        {
            if (Instance != null)
            {
                if (Instance.__Serializable.FloatValuePool.TryGetValue(Name, out var value))
                {
                    return value;
                }
                else return Fallback;
            }
            else return Fallback;
        }
        public static void Set(string Name, string Value)
        {
            if (Instance != null)
            {
                if (!Instance.__Serializable.StringValuePool.TryAdd(Name, Value))
                {
                    Instance.__Serializable.StringValuePool[Name] = Value;
                    Instance.StringValueWatcher.Invoke(new KVPair<string, string> { Key = Name, Value = Value });
                }
            }
        }
        public static void Set(string Name, bool Value)
        {
            if (Instance != null)
            {
                if (!Instance.__Serializable.BoolValuePool.TryAdd(Name, Value))
                {
                    Instance.__Serializable.BoolValuePool[Name] = Value;
                    Instance.BoolValueWatcher.Invoke(new KVPair<string, bool> { Key = Name, Value = Value });
                }
            }
        }
        public static void Set(string Name, float Value)
        {
            if (Instance != null)
            {
                if (!Instance.__Serializable.FloatValuePool.TryAdd(Name, Value))
                {
                    Instance.__Serializable.FloatValuePool[Name] = Value;
                    Instance.FloatValueWatcher.Invoke(new KVPair<string, float> { Key = Name, Value = Value });
                }
            }
        }
        private SerializableLevelRuntimeRegistry __Serializable = new SerializableLevelRuntimeRegistry();
        public IPureData ObtainData()
        {
            return __Serializable;
        }

        public void ApplyData(IPureData data)
        {
            if (data is SerializableLevelRuntimeRegistry S)
            {
                __Serializable = S;
            }
        }
    }
}
