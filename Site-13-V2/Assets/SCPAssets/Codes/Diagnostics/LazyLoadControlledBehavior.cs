using Site13Kernel.Core;
using System;

namespace Site13Kernel.Diagnostics
{
    [Serializable]
    public class LazyLoadControlledBehavior
    {
        public bool Loaded;
        public ControlledBehavior Behavior;
        public int FrameCount;
        public float DelayTime;
    }
}
