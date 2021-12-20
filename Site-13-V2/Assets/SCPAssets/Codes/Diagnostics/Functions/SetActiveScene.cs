using CLUNL.Utilities;
using Site13Kernel.GameLogic;
using System.Collections.Generic;

namespace Site13Kernel.Diagnostics.Functions
{
    public class SetActiveScene : IDiagnosticsFunction
    {
        public void Execute(List<Argument> arguments)
        {
            if (arguments == null)
            {
                Help();
                return;
            }
            if (arguments.Count == 0)
            {
                Help();
                return;
            }
            if (arguments[0].EntireArgument.ToUpper() == "--HELP")
            {
                Help();
                return;
            }
            else
            {
                if (int.TryParse(arguments[0].EntireArgument, out var i))
                {
                    SceneLoader.Instance.SetActive(i);
                    Debugger.CurrentDebugger.Log("Done.");

                }
                else
                {
                    Debugger.CurrentDebugger.LogError("Invalid Argument.");
                }

            }
        }

        public string GetCommandName()
        {
            return "setactivescene";
        }
        public List<string> GetAlias()
        {
            return new List<string>
            {
                GetCommandName(),"active"
            };
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("SetActiveScene <SceneID:int>");
            Debugger.CurrentDebugger.Log("\tSet a scene to active.");
        }
    }
}
