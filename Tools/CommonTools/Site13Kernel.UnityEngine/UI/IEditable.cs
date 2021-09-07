using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.UI
{
    public interface IEditable
    {
        public void SetCallback(Action<object> callback);
        public void SetValue(object obj);
        public void InitValue(object obj);
        public object GetValue();
    }
}
