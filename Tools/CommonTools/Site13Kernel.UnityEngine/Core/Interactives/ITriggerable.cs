using System;
using System.Collections.Generic;

namespace Site13Kernel.Core.Interactives
{
    public interface ITriggerable
    {
        List<Action> Callback { get; set; }
        void AddCallback(Action Callback);
    }
}
