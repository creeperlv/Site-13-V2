using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    public class LibInfo
    {
        public static Version GetVersion()
        {
            return typeof(LibInfo).Assembly.GetName().Version;
        }
    }
}
