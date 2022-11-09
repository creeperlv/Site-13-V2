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
        public static Site13Event<T> operator +(Site13Event<T> e, Action<T> a)
        {
            e.Add(a);
            return e;
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
        public static Site13Event<T, U> operator +(Site13Event<T, U> e, Action<T, U> a)
        {
            e.Add(a);
            return e;
        }
    }
    [Serializable]
    public class Site13Event<T, U, V> : ConnectableList<Action<T, U, V>>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T t, U u, V v)
        {
            foreach (var item in this)
            {
                item(t, u, v);
            }
        }
        public static Site13Event<T, U, V> operator +(Site13Event<T, U, V> e, Action<T, U, V> a)
        {
            e.Add(a);
            return e;
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
        public static Site13Event operator +(Site13Event e, Action a)
        {
            e.Add(a);
            return e;
        }
    }
}
