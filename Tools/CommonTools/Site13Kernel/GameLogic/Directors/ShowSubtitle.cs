using CLUNL.Localization;
using Site13Kernel.Data.Attributes;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Catalog("Speech")]
    [Serializable]
    public class ShowSubtitle : EventBase
    {
        public LocalizedString Content;
        public float Length;
    }
}
