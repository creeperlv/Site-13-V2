using CLUNL.Localization;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class ShowSubtitle : EventBase
    {
        public LocalizedString Content;
        public float Length;
    }
}
