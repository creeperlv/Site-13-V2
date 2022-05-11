using Site13Kernel.Core;
using Site13Kernel.Diagnostics;
using Site13Kernel.UI.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

namespace Site13Kernel.UI.Settings
{
    public class SettingsController : ControlledBehavior
    {
        public UniversalRenderPipelineAsset URP_Asset;
        public List<SettingsItem> SettingsItems;
        public AudioMixerGroup UI;
        public AudioMixerGroup UI_BGM;
        public AudioMixerGroup SFX;
        public AudioMixerGroup BGM;
        public override void Init()
        {
            URP_Asset=UniversalRenderPipeline.asset;
            
            foreach (var item in SettingsItems)
            {
                try
                {
                    item.Parent = this;
                    item.Init();
                }
                catch (System.Exception e)
                {
                    Debugger.CurrentDebugger.LogWarning($"Settings \"{item.SettingsItemID}\" init failed: {e}");
                }
            }
        }
    }
}
