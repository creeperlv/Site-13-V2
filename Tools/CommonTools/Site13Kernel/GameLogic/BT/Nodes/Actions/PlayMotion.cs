using Site13Kernel.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.GameLogic.BT.Nodes.Actions
{
    [Serializable]
    [Catalog("Actions>Animation")]
    public class PlayMotion : BTBaseNode
    {
        public string Trigger;
        public int Layer;
    }
    [Serializable]
    [Catalog("Actions>Animation")]
    public class PlayRandomMotion : BTBaseNode
    {
        public string MotionCollectionID;
    }
}
