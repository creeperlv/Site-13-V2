using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class EventBase
    {
        public bool UseEventTrigger;
        public bool UseSymbolInsteadOfEventTrigger;
        public string EventTriggerID;
        public float TimeDelay;
        public bool Repeatable;
    }
}