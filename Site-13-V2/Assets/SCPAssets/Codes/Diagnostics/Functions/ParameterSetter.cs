using CLUNL.Utilities;
using Site13Kernel.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Diagnostics.Functions
{
    public class ParameterSetter : IDiagnosticsFunction
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
                if (arguments.Count == 2)
                {
                    switch (arguments[0].EntireArgument.ToUpper())
                    {
                        case "ISINLEVEL":
                            {
                                if (bool.TryParse(arguments[1].EntireArgument, out var i))
                                {
                                    GameRuntime.CurrentGlobals.isInLevel = i;

                                }
                                else
                                {
                                    Debugger.CurrentDebugger.LogError("IsInLevel requires a bool argument.");
                                }
                            }
                            break;
                        case "CURSOR":
                            {
                                if (bool.TryParse(arguments[1].EntireArgument, out var i))
                                {
                                    Cursor.visible = i;

                                }
                                else
                                {
                                    Debugger.CurrentDebugger.LogError("Cursor requires a bool argument.");
                                }
                            }
                            break;
                        default:
                            break;
                    }

                }
                else
                {
                    Debugger.CurrentDebugger.LogError("Invalid Argument.");
                }

            }
        }

        public string GetCommandName()
        {
            return "set";
        }
        public void Help()
        {
            Debugger.CurrentDebugger.Log("set <key:string> <value:object>");
        }

    }
}
