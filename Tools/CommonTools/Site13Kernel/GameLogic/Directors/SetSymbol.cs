using Site13Kernel.Data.Attributes;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Catalog("Game Logic")]
    [Serializable]
    public class SetSymbol : EventBase
    {
        public string Key;
        public bool State;
    }
}