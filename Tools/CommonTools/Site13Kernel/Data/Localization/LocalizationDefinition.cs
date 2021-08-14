using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data.Localization
{
    [Serializable]
    public class LocalizationDefinition
    {
        public string LanguageCode;
        public string LanguageStringFile;
        public List<RefLocalizedImage> Textures=new List<RefLocalizedImage>();
        public List<RefLocalizedImage> Sprites=new List<RefLocalizedImage>();
    }
    [Serializable]
    public class RefLocalizedImage
    {
        public string Path;
    }
    [Serializable]
    public class LocalizationDefinitionCollection
    {
        public Dictionary<string,string> InstalledLocalizations=new Dictionary<string, string>();
    }
}
