using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Site13Kernel.Core.Interactives
{
    public class InteractiveBase : ControlledBehavior
    {
        public InteractiveMode InteractiveMode;
        public OperationMode OperationMode;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Operate()
        {

        }
    }
}
