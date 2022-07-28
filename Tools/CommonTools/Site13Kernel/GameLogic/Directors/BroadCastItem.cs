using CLUNL.Localization;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class BroadCastItem {
        public LocalizedString Title;
        public LocalizedString BroadContent;
        public LocalizedString Issuer;
    }

}