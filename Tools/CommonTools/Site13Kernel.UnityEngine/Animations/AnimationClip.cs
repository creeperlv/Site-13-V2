using Site13Kernel.Data;
using System;

namespace Site13Kernel.Animations
{
    [Serializable]
    public sealed class Site13AnimationClip : IDuplicatable<Site13AnimationClip>
    {
        public string Trigger;
        public float Length;
        public bool WaitUntilDone;

        public Site13AnimationClip Duplicate()
        {
            return new Site13AnimationClip
            {
                Trigger = Trigger,
                Length = Length,
                WaitUntilDone = WaitUntilDone
            };
        }
    }
}
