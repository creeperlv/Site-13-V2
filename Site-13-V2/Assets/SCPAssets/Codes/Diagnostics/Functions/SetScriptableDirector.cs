using CLUNL.Utilities;
using Site13Kernel.Core;
using Site13Kernel.GameLogic.Directors;
using System.Collections.Generic;

namespace Site13Kernel.Diagnostics.Functions
{
    public class SetScriptableDirector : IDiagnosticsFunction
    {
        public List<string> GetAlias()
        {
            return new List<string>
            {
                GetCommandName(),"scriptabledirector","sd","sdirector"
            };
        }

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
                if (bool.TryParse(arguments[0].EntireArgument, out var i))
                {
                    if (ScriptableDirector.Instance!= null)
                    {
                        ScriptableDirector.Instance.isRunning = i;
                        if (i)
                        {
                            GameRuntime.CurrentGlobals.isInLevel = true;
                        }
                        Debugger.CurrentDebugger.Log("Done.");

                    }
                    else
                    {
                        Debugger.CurrentDebugger.LogError("ScriptableDirector not exist!");
                        Debugger.CurrentDebugger.LogWarning("Are you in campaign level?");
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
            return "setscriptabledirector";
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("SetScriptableDirector <isRunning:bool>");
            Debugger.CurrentDebugger.Log("\tSet current scriptable director.");
        }
    }
}
