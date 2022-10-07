using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.BT.Nodes.Generic;
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
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
    [Serializable]
    public class RuntimeAnimationResource : ControlledBehavior
    {
        public static Dictionary<string, AnimationCollection> CachedResources = new Dictionary<string, AnimationCollection>(); 
        public bool OverrideDictionary = false;
        public bool ControlledBehaviorWorkflow = false;
        public KVList<string, List<AnimationCollection>> Animations = new KVList<string, List<AnimationCollection>>();
        void __init()
        {
            if (OverrideDictionary) CachedResources.Clear();
        }
        public void Start()
        {
            if (ControlledBehaviorWorkflow) return;
            __init();
        }
        public override void Init()
        {
            if (ControlledBehaviorWorkflow)
            {
                __init();
            }
        }
    }
}
