using CLUNL.Localization;
using System;
using UnityEngine;

namespace Site13Kernel.Data.Achievements
{
    [Serializable]
    public class HLAchievementItem: AchievementItemBase
    {
        public LocalizedString Title;
        public LocalizedString Content;
        public Sprite Icon;
    }
}
