using Site13Kernel.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core
{
    [Serializable]
    public class GameGlobals
    {
        public string AppData;
        public AudioSource MainUIBGM;
        public GameDefinition CurrentGameDef;

        public bool isDebugFunctionEnabled=false;
        public void Init()
        {
            AppData = Application.persistentDataPath;
            CurrentGameDef = new GameDefinition();
#if DEBUG
            isDebugFunctionEnabled = true;
#endif
        }
    }
}
