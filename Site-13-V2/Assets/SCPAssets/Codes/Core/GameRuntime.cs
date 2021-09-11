using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class GameRuntime 
    {

        public static GameLocals CurrentLocals;
        public static GameGlobals CurrentGlobals=new GameGlobals();
        public static Transform BulletHolder;
    }
}
