using CLUNL.Utilities;
using Site13Kernel.Core;
using System.Collections.Generic;

namespace Site13Kernel.Diagnostics.Functions
{
    public class UIBGM : IDiagnosticsFunction
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
                if (bool.TryParse(arguments[0].EntireArgument, out var b))
                {
                    if(b)
                        GameRuntime.CurrentGlobals.MainUIBGM.Play();
                    else
                        GameRuntime.CurrentGlobals.MainUIBGM.Pause();

                }
                else
                {
                    Debugger.CurrentDebugger.LogError("Invalid Argument.");
                }

            }
        }

        public string GetCommandName()
        {
            return "uibgm";
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("uibgm <play:bool>");
        }
    }
}
