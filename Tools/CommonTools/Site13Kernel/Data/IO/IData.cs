using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Site13Kernel.Data.IO
{
    public interface IData:ISerializable
    {
        void Save();
        void Load();
    }
}
