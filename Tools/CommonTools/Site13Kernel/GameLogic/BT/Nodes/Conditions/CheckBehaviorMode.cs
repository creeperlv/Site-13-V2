using Site13Kernel.Data.Attributes;
using Site13Kernel.GameLogic.BT.Nodes.Actions;
using System;

namespace Site13Kernel.GameLogic.BT.Nodes.Conditions
{
    [Serializable]
    [Catalog("Conditions")]
    public class CheckBehaviorMode : ConditionNode
    {
        public BehaviorMode TargetMode;
    }
}
