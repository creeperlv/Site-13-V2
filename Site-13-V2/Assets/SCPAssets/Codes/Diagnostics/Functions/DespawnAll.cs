using CLUNL.Utilities;
using Site13Kernel.GameLogic.AI;
using System.Collections.Generic;

namespace Site13Kernel.Diagnostics.Functions
{
    public class DespawnAll : IDiagnosticsFunction
    {

        public void Execute(List<Argument> arguments)
        {
            for (int i = AIController.CurrentController._OnRefresh.Count; i >= 0; i--)
            {
                var item = AIController.CurrentController._OnRefresh[i] as AICharacter;
                AIController.CurrentController.DestoryAICharacter(item);
            }
        }
        public string GetCommandName()
        {
            return "DespawnAll";
        }
        public List<string> GetAlias()
        {
            return new List<string> { GetCommandName(), "DespawnAll" };
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("listbiodef");
        }
    }
}
