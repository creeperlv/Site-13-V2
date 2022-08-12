using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Project.Core
{
    public class CommonProperties
    {
        public static string Query(Property property, LoadedProject LP,string fallback)
        {
            var conf = LP.ObtainCurrentConfiguration();
            switch (property)
            {
                case Property.Output:
                    return conf.Query("Output", fallback);
                case Property.TargetType:
                    return conf.Query("TargetType", fallback);
                default:
                    break;
            }
            return "";
        }
    }
    public enum Property
    {
        Output,TargetType
    }
}
