using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Diagnostics.Warns
{
    public class FallBackToMainMenuWarn: ISite13Warn
    {
        public override string ToString()
        {
            return "Falling back to Main Menu...";
        }
    }
    public interface ISite13Warn
    {

    }
}
