using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.GameLogic;
using Site13Kernel.GameLogic.FPS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

namespace Site13Kernel.Core
{
    [Serializable]
    public class GameGlobals
    {
        public string AppData;
        public string OneTimeScript = null;
        public bool isDebugFunctionEnabled = false;
        public bool isPaused = false;

        public int NextCampaign;
        public int MainMenuSceneID;

        public int Scene_LevelLoader;
        public int Scene_LevelBase;
        public int Scene_WinScene;

        public int LayerExcludePlayerAndAirBlock;
        public int LayerExcludePlayerAndAirBlockAndEventTrigger;
        public int LayerExcludeAirBlock;

        public int PickupableLayer;
        public int PickupableTriggerLayer;
        public Dictionary<string, TextAsset> Scripts = new Dictionary<string, TextAsset>();
        public UniversalRenderPipelineAsset UsingAsset;
        public BulletSystem CurrentBulletSystem;
        public AudioSource MainUIBGM;
        public GameDefinition CurrentGameDef;
        public SubtitleController SubtitleController;
        public AudioMixerGroup UI;
        public AudioMixerGroup UI_BGM;
        public AudioMixerGroup SFX;
        public AudioMixerGroup BGM;
        public MissionDefinition CurrentMission;
        public EffectController CurrentEffectController
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => EffectController.CurrentEffectController;
        }
        public Dictionary<int, GameObject> GeneralPrefabMap;
        internal bool isInLevel = false;

        public void Init()
        {
            AppData = Application.persistentDataPath;
            CurrentGameDef = new GameDefinition();
            {
                LayerExcludePlayerAndAirBlockAndEventTrigger = ~LayerMask.GetMask("Player", "Air block", "EventTrigger");
                LayerExcludePlayerAndAirBlock = ~LayerMask.GetMask("Player", "Air block");
                LayerExcludeAirBlock = ~LayerMask.GetMask("Air block");
            }
            //#if DEBUG
            isDebugFunctionEnabled = true;
            //#endif
        }
    }
}
