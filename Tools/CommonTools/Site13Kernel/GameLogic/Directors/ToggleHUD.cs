using Site13Kernel.Data.Attributes;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Catalog("Player")]
    [Serializable]
    public class ToggleHUD : EventBase
    {
        public bool TargetState;
    }
}
