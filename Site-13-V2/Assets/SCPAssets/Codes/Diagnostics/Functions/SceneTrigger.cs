using CLUNL.Utilities;
using Site13Kernel.GameLogic;
using Site13Kernel.Utilities;
using System.Collections.Generic;

namespace Site13Kernel.Diagnostics.Functions
{
    public class SceneTrigger : IDiagnosticsFunction
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
                bool isShow = true;
                if (arguments.Count > 1)
                {
                    isShow = bool.Parse(arguments[1]);
                }
                if (int.TryParse(arguments[0].EntireArgument, out var i))
                {
                    if (isShow)
                        SceneLoader.Instance.ShowScene(i);
                    else SceneLoader.Instance.HideScene(i);

                }
                else
                {
                    int ii = SceneUtility.LookUp(arguments[0].EntireArgument);
                    if ((ii) != -1)
                    {
                        if(isShow)
                            SceneLoader.Instance.ShowScene(ii);
                        else SceneLoader.Instance.HideScene(ii);
                    }
                    else
                        Debugger.CurrentDebugger.LogError("Invalid Argument.");
                }

            }
        }

        public string GetCommandName()
        {
            return "trigger";
        }

        public List<string> GetAlias()
        {
            return new List<string>
            {
                GetCommandName(),"visible"
            };
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("trigger <SceneID:int> [Show:bool]");
            Debugger.CurrentDebugger.Log("\tShow or hide a scene.");
        }
    }
}
