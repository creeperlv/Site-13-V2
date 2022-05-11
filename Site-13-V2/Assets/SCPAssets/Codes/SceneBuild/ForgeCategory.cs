using CLUNL.Localization;
using System;
using System.Collections.Generic;
namespace Site13Kernel.SceneBuild
{
    [Serializable]
    public class ForgeCategory
    {
        public LocalizedString Name;
        public List<ForgeObject> objects;
    }
}