using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data
{
    [Serializable]
    public class MissionCollection
    {
        public string ID;
        public string FallbackName;
        public List<MissionDefinition> MissionDefinitions;
    }
    public enum WorkMode
    {
        Internal, ExternalFile, ExternalServer
    }
    [Serializable]
    public class MissionDefinition
    {
        public string TargetScript;
        public string NameID;
        public string DispFallback;
        public string DescFallback;
        public string ImageName;
    }
}
