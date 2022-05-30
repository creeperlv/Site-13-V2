using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class PlayAnimation : EventBase {
        public string ReferenceAnimator;
        public string StateName;
        public bool UseAsTrigger;
    }
}
