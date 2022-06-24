using Site13Kernel.Data.IO;
using System;

namespace Site13Kernel.GameLogic.AI
{
    [Serializable]
    public class GoalData : IPureData
    {
        public string ReferenceID;
        public float Range;
        public float Speed;
        public GoalData()
        {

        }
        public GoalData(string referenceID, float range, float speed)
        {
            ReferenceID = referenceID;
            Range = range;
            Speed = speed;
        }
    }
}
