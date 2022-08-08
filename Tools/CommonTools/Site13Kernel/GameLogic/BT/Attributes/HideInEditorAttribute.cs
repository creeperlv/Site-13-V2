using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.GameLogic.BT.Attributes
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class HideInEditorAttribute : Attribute
    {
        public HideInEditorAttribute()
        {
        }

    }
}
