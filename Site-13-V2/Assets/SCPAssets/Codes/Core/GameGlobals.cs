using Site13Kernel.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Site13Kernel.Core
{
    [Serializable]
    public class GameGlobals
    {
        public string AppData;
        public AudioSource MainUIBGM;
        public GameDefinition CurrentGameDef;
        public SubtitleController SubtitleController;
        public bool isDebugFunctionEnabled=false;
        public int NextCampaign;
        public int MainMenuSceneID;
        public UniversalRenderPipelineAsset UsingAsset;
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
