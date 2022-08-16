using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;

namespace Site13Kernel.GameLogic.AI
{
    [Serializable]
    public class Routine : IDuplicatable
    {
        public List<Goal> Steps;
        public bool Loop;
        public bool RandomizeNextHop;
        public int CurrentStep;
        public IDuplicatable Duplicate()
        {
            return new Routine { Steps = Steps.Duplicate(), Loop = Loop, RandomizeNextHop = RandomizeNextHop };
        }
    }
}
