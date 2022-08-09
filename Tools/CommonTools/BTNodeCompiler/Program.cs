using CLUNL.ConsoleAppHelper;

namespace BTNodeCompiler
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConsoleAppHelper.Colorful = true;
            ConsoleAppHelper.PreExecution = () =>
            {
                Output.OutLine("This tool is licensed under the MIT License.");
                Output.OutLine("You are using a preview software, it may unstable, please do not use it in production environment.");
            };
            ConsoleAppHelper.Init("BTNode");
            ConsoleAppHelper.Execute(args);
        }
    }

}