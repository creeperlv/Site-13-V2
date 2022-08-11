using CLUNL.ConsoleAppHelper;
using System;
namespace BTNodeBuilder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConsoleAppHelper.Init("BTNodeBuilder", "Behavior Tree Node Builder");
            ConsoleAppHelper.Colorful = true;
            ConsoleAppHelper.ExecutableName = "BTNodeBuilder";
            ConsoleAppHelper.Execute(args);
        }
    }
}