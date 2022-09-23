using Site13Kernel.Data;
using System;
using System.Collections.Generic;

namespace Site13Kernel.GameLogic.Character
{
    [Serializable]
    public class AnimationCollection
    {
        public string Name;
        public KVList<string, List<string>> MappedAnimations = new KVList<string, List<string>>();
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
