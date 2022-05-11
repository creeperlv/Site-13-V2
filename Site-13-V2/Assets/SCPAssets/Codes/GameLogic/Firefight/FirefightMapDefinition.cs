using CLUNL.Localization;
using System;

namespace Site13Kernel.GameLogic.Firefight
{
    [Serializable]
    public class FirefightMapDefinition
    {
        public int SceneID;
        public string Image;
        public LocalizedString DisplayName;
        public LocalizedString Description;
    }
}
