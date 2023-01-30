using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using xUI.Core.Abstraction;

namespace xUI.Core.Helpers
{
    public static class PropertyHelper
    {
        public static void LayoutAlignment(this IxUILayoutable layoutable, string value, bool isVert)
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
        public static void Size(this ISize sizable, string value)
        {
            var g = value.Split(',');
            sizable.Size = new Vector2(
                float.Parse(g[0]),
                float.Parse(g[1]));
        }
    }
}
