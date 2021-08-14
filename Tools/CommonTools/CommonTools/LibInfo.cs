using Site13Kernel.Diagnostics;
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
        public static Version GetKernelVersion()
        {
            return typeof(Debugger).Assembly.GetName().Version;
        }
        public static string GetAboutString(string ProductName)
        {
            return string.Format(Strings.AboutString, ProductName, GetVersion().ToString(),GetKernelVersion().ToString());
        }
    }
}
