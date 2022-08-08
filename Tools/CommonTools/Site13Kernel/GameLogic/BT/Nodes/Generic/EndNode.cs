using Site13Kernel.Data.Attributes;
using Site13Kernel.GameLogic.BT.Attributes;
using System;

namespace Site13Kernel.GameLogic.BT.Nodes.Generic
{
    [Serializable]
    [Catalog("Sequence")]
    public class End: BTBaseNode
    {
    }
    [Serializable]
    [HideInEditor]
    [Catalog("Sequence")]
    public class Selector : BTBaseNode
    {

    }
}
