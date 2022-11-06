using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;

namespace Site13Kernel.Animations
{
    [Serializable]
    public class RuntimeAnimationResource : ControlledBehavior
    {
        public static Dictionary<string, BipedAnimationCollection> CachedResources = new Dictionary<string, BipedAnimationCollection>(); 
        public bool OverrideDictionary = false;
        public bool ControlledBehaviorWorkflow = false;
        public KVList<string, List<AnimationCollection>> Animations = new KVList<string, List<AnimationCollection>>();
        void __init()
        {
            if (OverrideDictionary) CachedResources.Clear();
            foreach (var item in Animations.PrefabDefinitions)
            {
                CachedResources.Add(item.Key, item.Value);
            }
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
