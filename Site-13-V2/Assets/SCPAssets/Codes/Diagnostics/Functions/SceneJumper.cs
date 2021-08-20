using CLUNL.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Site13Kernel.Diagnostics.Functinos
{
    public class SceneJumper : IDiagnosticsFunction
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
                bool isAdditive=false;
                if (arguments.Count > 1)
                {
                    isAdditive = bool.Parse(arguments[1]);
                }
                SceneManager.LoadScene(arguments[0].EntireArgument, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            }
        }

        public string GetCommandName()
        {
            return "jump";
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("SceneJumper <SceneName:string> [IsAdditive:bool]");
        }
    }
    public interface IDiagnosticsFunction
    {
        string GetCommandName();
        void Help();
        void Execute(List<Argument> arguments);
    }
}
