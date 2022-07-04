using Site13Kernel.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Site13Kernel.Core
{
    [Serializable]
    public class Site13Event<T> : ConnectableList<Action<T>>
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
    public class Site13Event<T, U> : ConnectableList<Action<T, U>>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T t, U u)
        {
            foreach (var item in this)
            {
                item(t, u);
            }
        }
    }
    [Serializable]
    public class Site13Event : ConnectableList<Action>
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
