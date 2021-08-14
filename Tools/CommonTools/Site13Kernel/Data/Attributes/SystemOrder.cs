using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    sealed class SystemOrderAttribute : Attribute
    {
        public SystemOrderAttribute(int Order)
        {
            this.Order = Order;

        }


        // This is a named argument
        public int Order
        {
            get; set;
        }
    }
}
