using Site13Kernel.Data.Attributes;
using System;

namespace Site13Kernel.GameLogic.BT.Nodes.Actions
{
    [Serializable]
    [Catalog("Actions>Basic Logic")]
    public class SetBehaviorMode : BTBaseNode
    {
        public BehaviorMode TargetMode;
    }
    public enum BehaviorMode
    {
        Goal,Fight,Search
    }
}
