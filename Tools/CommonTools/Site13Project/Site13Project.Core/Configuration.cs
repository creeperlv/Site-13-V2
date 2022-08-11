using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;

namespace Site13Project.Core
{
    [Serializable]
    public class Configuration : IDuplicatable
    {
        public string Condition = "";
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
        public string Query(string Key, string fallback = "")
        {
            if (Properties.TryGetValue(Key, out string Value))
            {
                return Value;
            }
            else
            {
                return fallback;
            }
        }
        public bool TryQuery(string Key, out string Output, string Fallback = "")
        {
            if (Properties.TryGetValue(Key, out string Value))
            {
                Output = Value;
                return true;
            }
            else
            {
                Output = Fallback;
                return false;
            }
        }
        public bool CheckCondition(params string[] conditions)
        {
            if (Condition == string.Empty) return true;
            foreach (var item in conditions)
            {
                if (item.ToUpper() == Condition.ToUpper()) return true;
            }
            return false;
        }
        public void Combine(Configuration configuration)
        {
            DictionaryOperations.Merge(ref Properties, configuration.Properties);
        }
        public IDuplicatable Duplicate()
        {
            return JsonUtilities.Deserialize<Configuration>(JsonUtilities.Serialize(this));
        }
    }

}
