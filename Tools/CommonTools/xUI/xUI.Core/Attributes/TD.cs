using System;
using System.Collections.Generic;
using System.Text;

namespace xUI.Core.Attributes
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class TODOAttribute : Attribute
    {
        public TODOAttribute()
        {
        }
    }
}
