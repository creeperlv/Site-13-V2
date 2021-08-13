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
        public AudioSource MainUIBGM;
        public GameDefinition CurrentGameDef;
        public void Init()
        {
            CurrentGameDef = new GameDefinition();
        }
    }
}