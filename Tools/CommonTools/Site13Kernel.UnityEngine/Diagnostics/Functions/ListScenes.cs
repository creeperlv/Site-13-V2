using CLUNL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

namespace Site13Kernel.Diagnostics.Functions
{
    public class ListScenes : IDiagnosticsFunction
    {
        public void Execute(List<Argument> arguments)
        {
            var Count=SceneManager.sceneCountInBuildSettings;
            Debugger.CurrentDebugger.Log($"Scene Count:{Count}");
            for (int i = 0; i < Count; i++)
            {
                var Name=SceneUtility.GetScenePathByBuildIndex(i).Split('/').Last();
                Debugger.CurrentDebugger.Log($"{i} > {Name.Substring(0,Name.Length-6)}");
            }
        }
        public string GetCommandName()
        {
            return "List-All-Scenes";
        }
        public List<string> GetAlias()
        {
            return new List<string> { "scenes", "ls-sc" };
        }
        public void Help()
        {
            Debugger.CurrentDebugger.Log($"List-All-Scenes\tList all scenes included in the build.");
        }
    }
}
