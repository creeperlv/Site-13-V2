using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core
{
    public static class GameRuntime
    {
        public static bool isServer = false;
        public static GameLocals CurrentLocals;
        public static GameGlobals CurrentGlobals = new GameGlobals();
        public static Transform BulletHolder;
        static GameRuntime()
        {
            isServer = Application.isBatchMode;
            if (!isServer)
            {
                var args = Environment.GetCommandLineArgs();
                foreach (var item in args)
                {
                    if (item.ToUpper() == "--SERVER")
                    {
                        isServer = true;
                    }
                }
            }
        }
    }
}
