using CLUNL.Utilities;
using Site13Kernel.GameLogic;
using Site13Kernel.Utilities;
using System.Collections.Generic;

namespace Site13Kernel.Diagnostics.Functions
{
    public class SceneUnloader : IDiagnosticsFunction
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
                    SceneLoader.Instance.Unload(i);

                }
                else
                {
                    int ID = -1;
                    if ((ID = SceneUtility.LookUp(arguments[0].EntireArgument)) != -1)
                    {
                        SceneLoader.Instance.Unload(ID);
                    }
                    else
                        Debugger.CurrentDebugger.LogError("Invalid Argument.");
                }

            }
        }

        public List<string> GetAlias()
        {
            return new List<string>
            {
                GetCommandName(),"unload"
            };
        }

        public string GetCommandName()
        {
            return "unloader";
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("unloader <SceneID:int>");
            Debugger.CurrentDebugger.Log("Unload certain scene.");
        }
    }
}
