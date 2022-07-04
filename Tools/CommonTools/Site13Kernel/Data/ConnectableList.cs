using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data
{
    public class ConnectableList<T> : List<T>
    {
        public void ConnectAfterEnd(List<T> __l)
        {
            foreach (var item in __l)
            {
                this.Add(item);
            }
        }
        public void ConnectBeforeStart(List<T> __l)
        {
            this.InsertRange(0, __l);
        }
    }
}
