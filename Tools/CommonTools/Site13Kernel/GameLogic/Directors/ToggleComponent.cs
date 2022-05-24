using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class ToggleComponent : EventBase
    {
        public string ComponentID;
        public bool TargetState;
    }
}
