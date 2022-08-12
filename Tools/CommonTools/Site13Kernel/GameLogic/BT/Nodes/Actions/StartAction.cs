using Site13Kernel.Data.Attributes;
using System;

namespace Site13Kernel.GameLogic.BT.Nodes.Actions
{
    [Serializable]
    [Catalog("Actions>Basic Logic")]
    public class StartAction: BTBaseNode
    {
        public float ActionLength;
    }
}
