using Site13Kernel.Data.Attributes;
using System;

namespace Site13Kernel.GameLogic.BT.Nodes.Generic
{
    [Serializable]
    [Catalog("Sequence")]
    public class End : BTBaseNode
    {
    }
    [Serializable]
    [Catalog("Sequence")]
    public class Wait : BTBaseNode
    {
        public int TickCount;
    }
    [Serializable]
    [Catalog("Sequence")]
    public class WaitTime : BTBaseNode
    {
        public float Time;
    }
}
