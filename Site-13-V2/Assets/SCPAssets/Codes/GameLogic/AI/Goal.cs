using System;
using System.Collections;
using System.Collections.Generic;
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
