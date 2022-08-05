using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.GameLogic.BT.Nodes
{
    [Serializable]
    public class BTBaseNode
    {
        public bool isStart;
        public bool isEnd;
        public BTBaseNode NextNode;
    }
}
