using Site13Kernel.Data;
using Site13Kernel.GameLogic.BT.Nodes.Generic;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Animations
{
    [Serializable]
    public class AnimationCollection
    {
        public string Name;
        public RuntimeAnimatorController ControllerToUse;
        public KVList<string, List<Site13AnimationClip>> MappedAnimations = new KVList<string, List<Site13AnimationClip>>();
        public Dictionary<string, List<Site13AnimationClip>> __mapped_animations = new Dictionary<string, List<Site13AnimationClip>>();
        internal void Convert()
        {
            __mapped_animations = MappedAnimations.ObtainMap();
            MappedAnimations = null;
        }
        public Site13AnimationClip ObtainAnimationTrigger(string Trigger)
        {
            if (__mapped_animations.ContainsKey(Trigger))
            {
                var L = __mapped_animations[Trigger];
                var clip = ListOperations.ObtainOne(L);
                return clip;
            }
            return new Site13AnimationClip
            {
                Trigger = Trigger,
                Length = -1, 
                WaitUntilDone=false
            };
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
