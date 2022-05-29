using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.RuntimeScenes
{
    public class LevelRuntimeRegistry : MonoBehaviour
    {
        public static LevelRuntimeRegistry Instance;
        public List<KVPair<string, string>> PredefinedStringValues;
        public List<KVPair<string, bool>> PredefinedBoolValues;
        public List<KVPair<string, float>> PredefinedFloatValues;
        public Dictionary<string, string> StringValuePool;
        public Dictionary<string, bool> BoolValuePool;
        public Dictionary<string, float> FloatValuePool;
        public void Start()
        {
            Instance = this;
            StringValuePool = CollectionUtilities.ToDictionary(PredefinedStringValues);
            BoolValuePool = CollectionUtilities.ToDictionary(PredefinedBoolValues);
            FloatValuePool = CollectionUtilities.ToDictionary(PredefinedFloatValues);
        }
        public static string QueryString(string Name, string Fallback = null)
        {
            if (Instance != null)
            {
                if (Instance.StringValuePool.TryGetValue(Name, out string Value))
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
                if (Instance.BoolValuePool.TryGetValue(Name, out var value))
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
                if (Instance.FloatValuePool.TryGetValue(Name, out var value))
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
                if (!Instance.StringValuePool.TryAdd(Name, Value))
                {
                    Instance.StringValuePool[Name] = Value;
                }
            }
        }
        public static void Set(string Name, bool Value)
        {
            if (Instance != null)
            {
                if (!Instance.BoolValuePool.TryAdd(Name, Value))
                {
                    Instance.BoolValuePool[Name] = Value;
                }
            }
        }
        public static void Set(string Name, float Value)
        {
            if (Instance != null)
            {
                if (!Instance.FloatValuePool.TryAdd(Name, Value))
                {
                    Instance.FloatValuePool[Name] = Value;
                }
            }
        }
    }
}
