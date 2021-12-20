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
        public static int LookUp(string Name)
        {
            switch (Name.ToUpper())
            {
                case "LEVELLOADER":
                    return GameRuntime.CurrentGlobals.Scene_LevelLoader;

                case "MACAU":
                    return 3;
                case "LEVELBASE":
                    return GameRuntime.CurrentGlobals.Scene_LevelBase;

                default:
                    break;
            }
            return -1;
        }
    }
}
