using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Attributes
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class TODOAttribute : Attribute
    {
        public TODOAttribute()
        {
        }
    }
}
