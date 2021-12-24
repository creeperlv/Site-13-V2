using CLUNL.Utilities;
using Site13Kernel.GameLogic;
using System.Collections.Generic;

namespace Site13Kernel.Diagnostics.Functions
{
    public class Show : IDiagnosticsFunction
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
                    SceneLoader.Instance.ShowScene(i);

                }

            }
        }

        public string GetCommandName()
        {
            return "show";
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("show <SceneID:int> [Show:bool]");
            Debugger.CurrentDebugger.Log("\tShow a scene.");
        }
    }
    public class Hide : IDiagnosticsFunction
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
                    SceneLoader.Instance.HideScene(i);

                }

            }
        }

        public string GetCommandName()
        {
            return "hide";
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("hide <SceneID:int> [Show:bool]");
            Debugger.CurrentDebugger.Log("\tHide a scene.");
        }
    }
}
