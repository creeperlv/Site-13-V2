using CLUNL.Utilities;
using Site13Kernel.GameLogic;
using System.Collections.Generic;

namespace Site13Kernel.Diagnostics.Functions
{
    public class SetStickScene : IDiagnosticsFunction
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
                bool isStick = true;
                if (arguments.Count > 1)
                {
                    isStick = bool.Parse(arguments[1]);
                }
                if (int.TryParse(arguments[0].EntireArgument, out var i))
                {
                    SceneLoader.Instance.SetStick(i, isStick);

                }
                else
                {
                    Debugger.CurrentDebugger.LogError("Invalid Argument.");
                }

            }
        }

        public string GetCommandName()
        {
            return "setstickscene";
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("setstickscene <SceneID:int> [IsStick:bool]");
            Debugger.CurrentDebugger.Log("\tSet a scene stick or not.");
        }
    }
    //public interface IDiagnosticsFunction
    //{
    //    string GetCommandName();
    //    void Help();
    //    void Execute(List<Argument> arguments);
    //}
}
