using CLUNL.Localization;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class IssueBroadcast : EventBase
    {
        public BroadCastItem BroadCast;
    }
    [Serializable]
    public class BroadCastItem {
        public LocalizedString Title;
        public LocalizedString BroadContent;
        public LocalizedString Issuer;
    }

}