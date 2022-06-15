using CLUNL.Utilities;
using Site13Kernel.Core;
using Site13Kernel.GameLogic.CampaignScripts;
using Site13Kernel.GameLogic.Firefight;
using System.Collections.Generic;

namespace Site13Kernel.Diagnostics.Functions
{
    public class SetFixedDirector : IDiagnosticsFunction
    {
        public List<string> GetAlias()
        {
            return new List<string>
            {
                GetCommandName(),"fixeddirector","fd","fdirector"
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
                    if (FixedDirector.CurrentDirector != null)
                    {
                        FixedDirector.CurrentDirector.isRunning = i;
                        if (i)
                        {
                            GameRuntime.CurrentGlobals.isInLevel = true;
                        }
                        Debugger.CurrentDebugger.Log("Done.");

                    }
                    else
                    {
                        Debugger.CurrentDebugger.LogError("FixedDirector not exist!");
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
            return "setfixeddirector";
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("SetFixedDirector <isRunning:bool>");
            Debugger.CurrentDebugger.Log("\tSet current director.");
        }
    }
    public class StartFirefightDirector : IDiagnosticsFunction
    {
        public List<string> GetAlias()
        {
            return new List<string>
            {
                GetCommandName(),"firefightdirector","ffd","ffdirector"
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
                if (FirefightDirector.Instance != null)
                {
                    GameRuntime.CurrentGlobals.isInLevel = true;
                    FirefightDirector.Instance.StartFirefight();
                }
            }
            if (arguments[0].EntireArgument.ToUpper() == "--HELP")
            {
                Help();
                return;
            }
            else
            {

                if (FirefightDirector.Instance != null)
                {
                    GameRuntime.CurrentGlobals.isInLevel = true;
                    FirefightDirector.Instance.StartFirefight();
                }
            }
        }

        public string GetCommandName()
        {
            return "startfirefightdirector";
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("startfirefightdirector");
            Debugger.CurrentDebugger.Log("\tStart firefight director.");
        }
    }
}
