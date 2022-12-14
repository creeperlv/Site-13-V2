using Site13Kernel.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Character
{
    [Serializable]
    public class AnimationCollection
    {
        public string Name;
        public RuntimeAnimatorController ControllerToUse;
        public KVList<string, List<AnimationClip>> MappedAnimations = new KVList<string, List<AnimationClip>>();
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
    [Serializable]
    public sealed class AnimationClip {
        public string Trigger;
        public float Length;
    }

}
