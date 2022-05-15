using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class ToggleObject : EventBase
    {
        public string ObjectID;
        public bool TargetState;
    }
}
