using System;

namespace Site13Kernel.Animations
{
    [Serializable]
    public sealed class Site13AnimationClip
    {
        public string Trigger;
        public float Length;
        public bool WaitUntilDone;
    }
}
