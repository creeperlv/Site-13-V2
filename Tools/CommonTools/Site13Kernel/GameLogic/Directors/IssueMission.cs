using CLUNL.Localization;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class IssueMission : EventBase
    {
        public LocalizedString MissionText;
    }
}