using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class TogglePlayer : EventBase
    {
        public bool TargetState;
    }
}