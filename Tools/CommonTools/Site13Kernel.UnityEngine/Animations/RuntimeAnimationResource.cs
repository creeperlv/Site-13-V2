using Site13Kernel.Core;
using Site13Kernel.Data;
using System;
using System.Collections.Generic;

namespace Site13Kernel.Animations
{
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
