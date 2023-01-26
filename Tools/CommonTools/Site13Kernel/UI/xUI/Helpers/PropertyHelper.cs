using Site13Kernel.UI.xUI.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.UI.xUI.Helpers
{
    public class PropertyHelper
    {
        public static void LayoutAlignment(IxUILayoutable layoutable, string value, bool isVert)
        {
            if (Enum.TryParse(typeof(xUIAlignment), value, out var align))
            {
                if (isVert)
                {
                    layoutable.VerticalAlignment = (xUIAlignment)align;
                }
                else
                {
                    layoutable.HorizontalAlignment = (xUIAlignment)align;
                }
            }
        }
        
    }
}
