using System;
using System.Collections.Generic;

namespace Site13Kernel.Animations
{
    [Serializable]
    public class BipedAnimationCollection
    {
        public Dictionary<string, AnimationCollection> Animations=new Dictionary<string, AnimationCollection>();
        public static BipedAnimationCollection FromListAnimationCollection(List<AnimationCollection> animationCollections)
        {
            var c=new BipedAnimationCollection { };
            foreach (var animationCollection in animationCollections)
            {
                animationCollection.Convert();
                c.Animations.Add(animationCollection.Name, animationCollection);
            }
            return c;
        }
        public static implicit operator BipedAnimationCollection (List<AnimationCollection> animationCollections)
        {
            return FromListAnimationCollection (animationCollections);
        }
    }
}
