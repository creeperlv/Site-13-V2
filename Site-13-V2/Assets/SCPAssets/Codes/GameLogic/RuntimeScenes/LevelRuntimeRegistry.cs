using Newtonsoft.Json;
using Site13Kernel.Data;
using Site13Kernel.Data.IO;
using Site13Kernel.Data.Serializables;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.RuntimeScenes
{
    public class LevelRuntimeRegistry : MonoBehaviour,IContainsPureData
    {
        public static LevelRuntimeRegistry Instance;
        [JsonIgnore]
        public List<KVPair<string, string>> PredefinedStringValues;
        [JsonIgnore]
        public List<KVPair<string, bool>> PredefinedBoolValues;
        [JsonIgnore]
        public List<KVPair<string, float>> PredefinedFloatValues;
        public void Start()
        {
            Instance = this;
            __Serializable.StringValuePool = CollectionUtilities.ToDictionary(PredefinedStringValues);
            __Serializable.BoolValuePool = CollectionUtilities.ToDictionary(PredefinedBoolValues);
            __Serializable.FloatValuePool = CollectionUtilities.ToDictionary(PredefinedFloatValues);
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
                }
            }
        }
        private SerializableLevelRuntimeRegistry __Serializable=new SerializableLevelRuntimeRegistry();
        public IPureData ObtainData()
        {
            return __Serializable;
        }

        public void ApplyData(IPureData data)
        {
            if(data is SerializableLevelRuntimeRegistry S)
            {
                __Serializable = S;
            }
        }
    }
}
