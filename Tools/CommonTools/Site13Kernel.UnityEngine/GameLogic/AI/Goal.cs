using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.GameLogic.AI
{
    [Serializable]
    public class Goal
    {
        public float Range;
        public float Speed;
        public Transform Target;
    }
}
