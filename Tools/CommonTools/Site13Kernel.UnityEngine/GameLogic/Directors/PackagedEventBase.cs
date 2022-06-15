using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.GameLogic.Directors
{
    [Serializable]
    public class PackagedEventBase
    {
        public EventBase RawEvent;
        public bool isIgnored;
        public bool Executed;
        public float TimeD;
    }
}
