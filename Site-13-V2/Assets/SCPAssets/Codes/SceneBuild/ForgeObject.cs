using CLUNL.Localization;
using Site13Kernel.Data;
using System;
namespace Site13Kernel.SceneBuild
{
    [Serializable]
    public class ForgeObject
    {
        public LocalizedString Name;
        public PrefabReference Prefab;
    }
}