using System;

namespace xUI.Core.Data
{
    [Serializable]
    public class Property:IEquatable<Property>
    {
        public string Key;
        public object Value;

        public bool Equals(Property other)
        {
            return Key == other.Key;
        }
    }
}
