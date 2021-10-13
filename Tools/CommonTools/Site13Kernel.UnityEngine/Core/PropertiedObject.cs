using Site13Kernel.UI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Site13Kernel.Core
{
    public class PropertiedObject : UnityEngine.Object, IPropertiedObject
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Property GetProperty(string name)
        {
            var __R = GetPropertyValue(name);
            if(__R==null)
            return null;
            return new Property { Key = name, Value = __R };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public virtual object GetPropertyValue(string name)
        {
            return null;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public virtual void SetProperty(string name, object value)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SetProperty(Property p)
        {
            SetProperty(p.Key, p.Value);
        }
    }
}
