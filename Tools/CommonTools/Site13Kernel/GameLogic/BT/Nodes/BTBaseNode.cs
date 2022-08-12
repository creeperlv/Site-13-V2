using Site13Kernel.GameLogic.BT.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.GameLogic.BT.Nodes
{
    [Serializable]
    public class BTBaseNode
    {
        public BTBaseNode NextNode = null;
    }
    [Serializable]
    [HideInEditor]
    public class ConditionNode:BTBaseNode
    {
        public bool RevertBool;
    }
}
