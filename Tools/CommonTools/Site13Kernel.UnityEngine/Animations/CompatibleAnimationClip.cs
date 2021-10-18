using System;
using UnityEngine;

namespace Site13Kernel.Animations
{
    [Serializable]
    public class CompatibleAnimationClip
    {
        public int HashCode;
        public bool isIgnoredApplication;
        public string AlternativeTrigger;
        public string AlternativeStateName;
        public AnimationClip AnimationClip;
    }
}
