using Site13Kernel.Data;
using Site13Kernel.Data.IO;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.GameLogic.AI
{
    [Serializable]
    public class Goal : IContainsPureData, IDuplicatable
    {
        public string ReferenceID;
        public float Range;
        public float Speed;
        public Transform Target;
        public void ApplyData(IPureData data)
        {
            if (data is GoalData d)
            {
                var _ref = Directors.ScriptableDirector.Instance.__ReferenceRountines[d.ReferenceID];
                ReferenceID = d.ReferenceID;
                Target = _ref.Target;
                Range = d.Range;
                Speed = d.Speed;
            }
        }

        public IDuplicatable Duplicate()
        {
            return new Goal()
            {
                ReferenceID = ReferenceID,
                Range = Range,
                Speed = Speed,
                Target = Target
            };
        }

        public IPureData ObtainData()
        {
            return new GoalData(ReferenceID, Range, Speed);
        }
    }
}
