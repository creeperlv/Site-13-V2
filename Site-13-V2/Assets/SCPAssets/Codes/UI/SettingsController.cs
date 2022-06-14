using Site13Kernel.Core;
using Site13Kernel.Diagnostics;
using Site13Kernel.UI.Settings;
using System;
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
        public GameObject SettingsMenu;
        public UIButton BackButton;
        public static SettingsController Instance;
        public ButtonGroup SettingsPageTabs;
        public CanvasGroup BlackCover;
        public float BlackCoverSpeed = 1;
        public List<GameObject> SettingsPages;
        public void Show(Action PreBackAction = null, Action PostBackAction = null)
        {
            SettingsMenu.SetActive(true);
            StartCoroutine(RevealCover());
            Camera.main.GetComponent<UniversalAdditionalCameraData>().cameraStack.Add(SettingsMenu.GetComponentInChildren<Camera>());
            BackButton.OnClick = () =>
            {
                if (PreBackAction != null) PreBackAction();
                StartCoroutine(Cover(PostBackAction));
            };
        }
        IEnumerator RevealCover()
        {
            while (BlackCover.alpha > 0)
            {
                yield return null;
                BlackCover.alpha -= Time.unscaledDeltaTime * BlackCoverSpeed;
            }
            BlackCover.gameObject.SetActive(false);
        }
        IEnumerator Cover(Action PostAction)
        {
            BlackCover.gameObject.SetActive(true);
            while (BlackCover.alpha < 1)
            {
                yield return null;
                BlackCover.alpha += Time.unscaledDeltaTime * BlackCoverSpeed;
            }
            yield return null;
            BlackCover.alpha = 1;
            SettingsMenu.SetActive(false);
            Camera.main.GetComponent<UniversalAdditionalCameraData>().cameraStack.Remove(SettingsMenu.GetComponentInChildren<Camera>());
            if (PostAction != null)
            {
                PostAction();
            }
        }
        public override void Init()
        {
            Instance = this;
            URP_Asset = UniversalRenderPipeline.asset;
            {
                SettingsPageTabs.Init();
                SettingsPageTabs.OnSelected = (i) =>
                {
                    foreach (var item in SettingsPages)
                    {
                        item.SetActive(false);
                    }
                    SettingsPages[i].SetActive(true);
                };
            }
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
