using Site13Kernel.Core;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data.Achievements
{
    public class AchievementList:ControlledBehavior
    {
        public static AchievementList Instance;
        public List<HLAchievementItem> Definitions;
        public List<AchievementItemBase> Completion;
        public override void Init()
        {
            Instance = this;
        }
    }
}
