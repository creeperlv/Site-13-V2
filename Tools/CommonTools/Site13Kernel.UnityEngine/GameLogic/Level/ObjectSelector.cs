using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Site13Kernel.GameLogic.Level
{
    public class ObjectSelector:ControlledBehavior
    {
        public bool ExecuteOnStart;
        public List<RandomObject> RandomObjects;
        public int SelectionCount;
        void Start()
        {
            if (ExecuteOnStart) Execute();
        }
        public override void Init()
        {
            if (!ExecuteOnStart) Execute();
        }
        void Execute()
        {
            SelectionCount = Math.Min(SelectionCount, RandomObjects.Count);
            for (int i = 0; i < SelectionCount; i++)
            {
                RandomObjects[ObtainOneCount()].TargetObject.SetActive(true);
            }
        }
        int ObtainOneCount()
        {
            var _i = Random.Range(0, RandomObjects.Count);
            if (RandomObjects[_i].TargetObject.activeInHierarchy) return ObtainOneCount();
            return _i;
        }
    }
    [Serializable]
    public class RandomObject
    {
        public GameObject TargetObject;
    }
}
