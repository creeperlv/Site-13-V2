using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site13Kernel.Utilities
{
    public class SceneUtility
    {
        public static Dictionary<string, int> Mapping=new Dictionary<string, int>();
        public static int LookUp(string Name)
        {
            switch (Name.ToUpper())
            {
                case "LEVELLOADER":
                    return GameRuntime.CurrentGlobals.Scene_LevelLoader;
                case "LEVELBASE":
                    return GameRuntime.CurrentGlobals.Scene_LevelBase;

                default:
                    if (Mapping.ContainsKey(Name.ToUpper()))
                    {
                        return Mapping[Name.ToUpper()];
                    }
                    else
                    {
                        var SCENE = UnityEngine.SceneManagement.SceneManager.GetSceneByName(Name);
                        if (SCENE != null)
                        {
                            return SCENE.buildIndex;
                        }
                    }
                    break;
            }
            return -1;
        }
    }
}
