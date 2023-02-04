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
    ///// <summary>
    ///// If return true, the process will break.
    ///// </summary>
    //[Serializable]
    //public class BreakableEvent : ConnectableList<Func<bool>>
    //{
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public bool Invoke()
    //    {
    //        foreach (var item in this)
    //        {
    //            if (item()) return true;
    //        }
    //        return false;
    //    }
    //    public static BreakableEvent operator +(BreakableEvent e, Func<bool> a)
    //    {
    //        e.Add(a);
    //        return e;
    //    }

    //}
    ///// <summary>
    ///// If return true, the process will break.
    ///// </summary>
    //[Serializable]
    //public class BreakableEvent<T> : ConnectableList<Func<T, bool>>
    //{
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public bool Invoke(T t)
    //    {
    //        foreach (var item in this)
    //        {
    //            if (item(t)) return true;
    //        }
    //        return false;
    //    }
    //    public static BreakableEvent<T> operator +(BreakableEvent<T> e, Func<T, bool> a)
    //    {
    //        e.Add(a);
    //        return e;
    //    }

    //}
    ///// <summary>
    ///// If return true, the process will break.
    ///// </summary>
    //[Serializable]
    //public class BreakableEvent<T,U> : ConnectableList<Func<T,U, bool>>
    //{
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public bool Invoke(T t,U u)
    //    {
    //        foreach (var item in this)
    //        {
    //            if (item(t,u)) return true;
    //        }
    //        return false;
    //    }
    //    public static BreakableEvent<T,U> operator +(BreakableEvent<T,U> e, Func<T,U, bool> a)
    //    {
    //        e.Add(a);
    //        return e;
    //    }

    //}
}
