using CLUNL.Localization;
using Site13Kernel.Data.Attributes;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Catalog("System Messages")]
    [Serializable]
    public class IssueMission : EventBase
    {
        public LocalizedString MissionText;
    }
}