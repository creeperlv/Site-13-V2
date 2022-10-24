using Site13Kernel.Assets.KoFMUST.Codes.Utilities;
using Site13Kernel.Core;
using Site13Kernel.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using ShadowQuality = UnityEngine.ShadowQuality;

namespace Site13Kernel.UEFI
{
    public class EFIInitializerFunction : UEFIBase
    {
        public GameObject BackgroundObject;
        public UniversalRenderPipelineAsset TargetAsset;
        public Configuration InitConfiguration;

        public AudioMixerGroup UI;
        public AudioMixerGroup UI_BGM;
        public AudioMixerGroup SFX;
        public AudioMixerGroup BGM;

        public int SceneID_LevelBase;
        public int SceneID_LevelLoader;
        public int SceneID_WinScene;
        public List<SceneMapping> SceneMappings;

        public override void Init()
        {
            GameEnv.DataPath = Application.persistentDataPath;
            GameEnv.CollisionDamageSpeedThreshold = InitConfiguration.CollisionDamageSpeedThreshold;
            GameEnv.CollisionDamageIntensity = InitConfiguration.CollisionDamageIntensity;

            Settings.Init();
            PlayerWeaponCoatings.Init();

            GameRuntime.CurrentGlobals.UI = UI;
            GameRuntime.CurrentGlobals.UI_BGM = UI_BGM;
            GameRuntime.CurrentGlobals.SFX = SFX;
            GameRuntime.CurrentGlobals.BGM = BGM;

            GameRuntime.CurrentGlobals.Init();
            GameRuntime.CurrentGlobals.UsingAsset = TargetAsset;
            GameRuntime.CurrentGlobals.PickupableLayer= LayerMask.NameToLayer("Pickupable-Item");
            GameRuntime.CurrentGlobals.PickupableTriggerLayer = LayerMask.NameToLayer("Pickupable-Item-Trigger");
            TargetAsset.renderScale = Settings.CurrentSettings.RenderScale/100f;
            {

                AudioUtility.SetVolume(UI.audioMixer, Settings.CurrentSettings.UI_SFX);
                AudioUtility.SetVolume(UI_BGM.audioMixer, Settings.CurrentSettings.UI_BGM);
                AudioUtility.SetVolume(SFX.audioMixer, Settings.CurrentSettings.SFX);
                AudioUtility.SetVolume(BGM.audioMixer, Settings.CurrentSettings.BGM);
            }
            GameRuntime.CurrentGlobals.Scene_LevelBase = SceneID_LevelBase;
            GameRuntime.CurrentGlobals.Scene_LevelLoader= SceneID_LevelLoader;
            GameRuntime.CurrentGlobals.Scene_WinScene = SceneID_WinScene;
            foreach (var item in SceneMappings)
            {
                foreach (var name in item.Names)
                {
                    Utilities.SceneUtility.Mapping.Add(name, item.ID);
                }
            }
            Instantiate(BackgroundObject);
        }
    }
    [Serializable]
    public class SceneMapping
    {
        public List<string> Names=new List<string>();
        public int ID;
    }
    [Serializable]
    public class Configuration
    {
        public float CollisionDamageSpeedThreshold;
        public float CollisionDamageIntensity;
    }
}
