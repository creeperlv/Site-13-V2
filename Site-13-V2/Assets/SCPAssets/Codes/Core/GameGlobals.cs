using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.GameLogic;
using Site13Kernel.GameLogic.FPS;
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
        public bool isDebugFunctionEnabled=false;
        public int NextCampaign;
        public int MainMenuSceneID;

        public UniversalRenderPipelineAsset UsingAsset;
        public BulletSystem CurrentBulletSystem;
        public AudioSource MainUIBGM;
        public GameDefinition CurrentGameDef;
        public SubtitleController SubtitleController;
        public EffectController CurrentEffectController;
        public Dictionary<int, GameObject> GeneralPrefabMap;
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
