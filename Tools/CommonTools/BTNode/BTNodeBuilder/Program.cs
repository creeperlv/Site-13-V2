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
            ConsoleAppHelper.PreExecution = () =>
            {
                Output.OutLine("This tool is licensed under the MIT License.");
                Output.OutLine("You are using a preview software, it may unstable, please do not use it in production environment.");
            };
            ConsoleAppHelper.Init("BTNodeBuilder", "Behavior Tree Node Builder");
            ConsoleAppHelper.Execute(args);
        }
    }
}