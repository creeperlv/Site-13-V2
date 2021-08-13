using Site13Kernel.Core;
using Site13Kernel.GameLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Site13Kernel.UI
{
    public class MainMenu : ControlledBehavior
    {
        #region 
        public Button SettingsButton;
        #endregion
        public Transform CampaignHolder;
        public GameObject CampaignButton;
        public List<ButtonedPage> pages=new List<ButtonedPage>();
        public override void Init()
        {
            {
                for (int i = CampaignHolder.childCount - 1; i >= 0; i--)
                {
                    Destroy(CampaignHolder.GetChild(i).gameObject);
                }
                CampaignButtonGroup group=new CampaignButtonGroup();
                if (GameRuntime.CurrentGlobals.CurrentGameDef != null)
                    if (GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections != null)
                        if (GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections.Count > 0)
                            foreach (var item in GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections[0].MissionDefinitions)
                            {
                                var b=Instantiate(CampaignButton, CampaignHolder);
                                var cb=b.GetComponent<CampaignButton>();
                                cb.Init(group, item);
                            }
            }
            if (GameRuntime.CurrentGlobals.MainUIBGM != null)
                if (!GameRuntime.CurrentGlobals.MainUIBGM.isPlaying)
                    GameRuntime.CurrentGlobals.MainUIBGM.Play();
            foreach (var item in pages)
            {
                foreach (var btn in item.Buttons)
                {
                    btn.onClick.AddListener(() =>
                    {
                        HideAllPages();
                        item.Show();
                    });
                }
            }
            SettingsButton.onClick.AddListener(() => {
                this.GetComponent<CanvasGroup>().interactable=false;
                SceneManager.LoadScene("SettingsUI", LoadSceneMode.Additive);
                //SettingsPage.OnBack = () => {
                //    this. GetComponent<CanvasGroup>().interactable = true;
                //};
            });
        }
        void HideAllPages()
        {
            foreach (var item in pages)
            {
                item.Hide();
            }
        }
    }
    [Serializable]
    public class ButtonedPage
    {
        public List<Button> Buttons;
        public GameObject Page;
        public void Hide()
        {
            Page.SetActive(false);
        }
        public void Show()
        {
            Page.SetActive(true);
        }
    }

}