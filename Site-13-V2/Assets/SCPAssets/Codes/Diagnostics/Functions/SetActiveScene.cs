using CLUNL.Utilities;
using Site13Kernel.GameLogic;
using Site13Kernel.Utilities;
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
                    Debugger.CurrentDebugger.Log($"Scene (ID:{i}) is now active.");

                }
                else
                {
                    var ID=SceneUtility.LookUp(arguments[0].EntireArgument);
                    if (ID != -1)
                    {
                        SceneLoader.Instance.SetActive(ID);
                        Debugger.CurrentDebugger.Log($"Scene (Name:{arguments[0].EntireArgument}=>ID={ID}) is now active");

                    }
                    else
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
            Debugger.CurrentDebugger.Log("SetActiveScene <SceneID:int>|<SceneName:string>");
            Debugger.CurrentDebugger.Log("\tSet a scene to active.");
        }
    }
}
