using CLUNL.Utilities;
using System.Collections.Generic;

namespace Site13Kernel.Diagnostics.Functions
{
    public class EraseAIs : IDiagnosticsFunction
    {
        public void Execute(List<Argument> arguments)
        {
            AIController.CurrentController.DestoryAllCharacters();
        }

        public string GetCommandName()
        {
            return "eraseais";
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("EraseAIs");
            Debugger.CurrentDebugger.Log("\tDestory all AIs.");
        }
    }
}
