using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.UI.xUI.Utilities
{
    public static class DataConversion
    {
        public static UnityEngine.Vector2 ToUnityVector(this System.Numerics.Vector2 value)
        {
            return new UnityEngine.Vector2(value.X, value.Y);
        }
    }
}
