using Site13Kernel.Data.Attributes;
using System;

namespace Site13Kernel.GameLogic.BT.Nodes.Conditions
{
    [Serializable]
    [Catalog("Conditions")]
    public class IsSpottedEnemy : ConditionNode
    {

    }
    [Serializable]
    [Catalog("Conditions")]
    public class GoalInRange : ConditionNode
    {
        public float Range;
    }
    [Serializable]
    [Catalog("Conditions")]
    public class GoalInPrimaryRange : ConditionNode
    {
    }
}
