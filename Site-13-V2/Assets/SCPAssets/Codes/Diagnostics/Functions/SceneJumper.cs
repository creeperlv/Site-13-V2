using CLUNL.Utilities;
using Site13Kernel.GameLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Site13Kernel.Diagnostics.Functions
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
                bool isAdditive = false;
                bool isShow = true;
                bool isStick = false;
                if (arguments.Count > 1)
                {
                    isAdditive = bool.Parse(arguments[1]);
                }
                if (arguments.Count > 2)
                {
                    isShow = bool.Parse(arguments[2]);
                }
                if (arguments.Count > 3)
                {
                    isStick = bool.Parse(arguments[3]);
                }
                if (int.TryParse(arguments[0].EntireArgument, out var i))
                {
                    SceneLoader.Instance.LoadScene(i, isShow, isAdditive, isStick);

                }
                else
                {
                    Debugger.CurrentDebugger.LogError("Invalid Argument.");
                }

            }
        }
        public List<string> GetAlias()
        {
            return new List<string>
            {
                GetCommandName(),"load"
            };
        }

        public string GetCommandName()
        {
            return "jump";
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("SceneJumper <SceneID:int> [IsAdditive:bool(false)] [IsShow:bool(true)] [IsStick:bool(false)]");
        }
    }
    //public interface IDiagnosticsFunction
    //{
    //    string GetCommandName();
    //    void Help();
    //    void Execute(List<Argument> arguments);
    //}
}
