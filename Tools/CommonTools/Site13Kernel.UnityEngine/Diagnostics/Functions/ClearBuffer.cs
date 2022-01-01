
using CLUNL.Utilities;
using System.Collections.Generic;

namespace Site13Kernel.Diagnostics.Functions
{
    public class ClearBuffer : IDiagnosticsFunction
    {

        public void Execute(List<Argument> arguments)
        {
            Debug.ClearBuffers();
        }

        public string GetCommandName()
        {
            return "Clear-Buffer";
        }

        public void Help()
        {
            Debug.Log("Clear-Buffer");
            Debug.Log("\tClear buffers if possible");
        }
        public List<string> GetAlias()
        {
            return new List<string> { "cls", "Console.Clear","clear", GetCommandName() };
        }
    }
}
