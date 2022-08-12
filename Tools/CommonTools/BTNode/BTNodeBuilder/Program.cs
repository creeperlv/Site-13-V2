using CLUNL.ConsoleAppHelper;
using CLUNL.Localization;
using System;
using System.Diagnostics;

namespace BTNodeBuilder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConsoleAppHelper.Colorful = true;
            var FI = new FileInfo(Process.GetCurrentProcess().MainModule!.FileName!);
            Language.SetDataPath(FI.Directory!.FullName);
            ConsoleAppHelper.ExecutableName = FI.Name;
            ConsoleAppHelper.Init("BTNodeBuilder", "Behavior Tree Node Builder");
            ConsoleAppHelper.Execute(args);
        }
    }
}