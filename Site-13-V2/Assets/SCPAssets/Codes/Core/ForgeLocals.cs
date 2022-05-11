using System;
using System.IO;

namespace Site13Kernel.Core
{
    [Serializable]
    public class ForgeLocals
    {
        public static ForgeLocals Instance = null;
        public FileInfo SceneDescriptionFile = null;
        public string BaseMap;
        public bool isReadOnly = false;
        public static void Config(ForgeLocals instance)
        {
            Instance = instance;
        }
        public static void DestoryCurrentLocal()
        {
            Instance = null;
        }
    }
}
