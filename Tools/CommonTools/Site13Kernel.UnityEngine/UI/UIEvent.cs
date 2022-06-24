using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Site13Kernel.UI
{
    [Serializable]
    public class UIEvent<T> : List<Action<T>>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T t)
        {
            foreach (var item in this)
            {
                item(t);
            }
        }
    }
    [Serializable]
    public class UIEvent<T,U> : List<Action<T,U>>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T t,U u)
        {
            foreach (var item in this)
            {
                item(t,u);
            }
        }
    }
    [Serializable]
    public class UIEvent : List<Action>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke()
        {
            foreach (var item in this)
            {
                item();
            }
        }
    }
}
