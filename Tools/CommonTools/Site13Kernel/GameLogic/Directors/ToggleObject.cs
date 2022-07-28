﻿using Site13Kernel.Data.Attributes;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Catalog("Game Logic")]
    [Serializable]
    public class ToggleObject : EventBase
    {
        public string ObjectID;
        public bool TargetState;
    }
}
