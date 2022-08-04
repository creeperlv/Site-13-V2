using CLUNL.Localization;
using Site13Kernel.Data.Attributes;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Catalog("Story")]
    [Serializable]
    public class ShowChapterNameEvent : EventBase
    {
        public LocalizedString ChapterName;
    }
}
