using System;

namespace Site13Kernel.Core.Interactives
{
    public interface ITriggerable
    {
        Action Callback { get; set; }
    }
}
