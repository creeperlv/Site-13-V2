using System;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Data
{
    [Serializable]
    public class KVPair<T,V>
    {
        public T Key;
        public V Value;
    }
}
