using System;
using System.Collections.Generic;
using System.Text;

namespace xUI.Core.Events
{
    public enum EventRaiser
    {
        User, Program
    }
    [Serializable]
    public class FocusEvent
    {
        public bool Interrupt;
        public bool PreventRaiseToParent;
        public EventRaiser EventRaiser;
    }
}
