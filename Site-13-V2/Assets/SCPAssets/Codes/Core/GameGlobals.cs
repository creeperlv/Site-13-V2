using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.GameLogic;
using Site13Kernel.GameLogic.FPS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Site13Kernel.Core
{
    [Serializable]
    public class GameGlobals
    {
        public string AppData;
        public bool isDebugFunctionEnabled = false;
        public int NextCampaign;
        public int MainMenuSceneID;

        public int LayerExcludePlayerAndAirBlock;
        public int LayerExcludeAirBlock;

        public UniversalRenderPipelineAsset UsingAsset;
        public BulletSystem CurrentBulletSystem;
        public AudioSource MainUIBGM;
        public GameDefinition CurrentGameDef;
        public SubtitleController SubtitleController;
        public EffectController CurrentEffectController
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => EffectController.CurrentEffectController;
        }
        public Dictionary<int, GameObject> GeneralPrefabMap;
        public void Init()
        {
            AppData = Application.persistentDataPath;
            CurrentGameDef = new GameDefinition();
            {
                int _000 = 1 << 10;// Layer 10, Air Block Layer.
                int _001 = 1 << 11;// Layer 11, Player Layer.
                LayerExcludePlayerAndAirBlock = _000 | _001;
                LayerExcludePlayerAndAirBlock = ~LayerExcludePlayerAndAirBlock;
                LayerExcludeAirBlock = ~_000;
            }
#if DEBUG
            isDebugFunctionEnabled = true;
#endif
        }
    }
}
