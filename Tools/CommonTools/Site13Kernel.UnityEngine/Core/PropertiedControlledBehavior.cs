using Site13Kernel.UI;
using System.Runtime.CompilerServices;

namespace Site13Kernel.Core
{
    public class PropertiedControlledBehavior : ControlledBehavior, IPropertiedObject
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Property GetProperty(string name)
        {
            var __R = GetPropertyValue(name);
            if (__R == null)
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
