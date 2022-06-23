using Site13Kernel.Core;
using Site13Kernel.Diagnostics;
using Site13Kernel.GameLogic;
using Site13Kernel.UI.Customizations;
using Site13Kernel.UI.Documents.PLN;
using Site13Kernel.UI.General;
using Site13Kernel.UI.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = Site13Kernel.Diagnostics.Debug;

namespace Site13Kernel.UI
{
    public class MainMenu : Page
    {
        #region 
        public UIButton SettingsButton;
        #endregion
        public Transform CampaignHolder;
        public GameObject CampaignButton;
        public List<ButtonedPage> pages = new List<ButtonedPage>();
        int SelectedPage = 0;
        public float AnimationSpeed = 4;
        public ButtonGroup SettingsPageTabs;
        public List<GameObject> SettingsPages;
        public Transform AboutContainer;
        public List<string> AboutDoc;
        public GameObject TextTemplate;
        public GameObject ImageTemplate;
        public UIButton StartButton;
        public UIButton ForgeButton;
        public UIButton FirefightButton;
        public UIButton CustomizationButton;
        public int CustomizationPage=1;
        public int LevelBase;
        public List<UIButton> CloseButtons=new List<UIButton>();
        public List<UIButton> ResetButtons=new List<UIButton>();
        CampaignButtonGroup group = new CampaignButtonGroup();
        public int FakePageID = 2;
        public float FadeDuration= 0.5f;
        public override void Init()
        {
            SettingsButton.OnClick = () => {
                StartCoroutine(WaitAndToSettings());
            };
            if (AboutContainer != null)
            {
                if (AboutDoc != null)
                {
                    try
                    {
                        PLNEngineCore.Init(TextTemplate, ImageTemplate);
                        PLNEngineCore.SetStyle(new StylingConfiguration());
                        PLNEngineCore.View(AboutContainer, AboutDoc, Color.white, 36);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            if (GameRuntime.CurrentGlobals.MainUIBGM != null)
                if (!GameRuntime.CurrentGlobals.MainUIBGM.isPlaying)
                    GameRuntime.CurrentGlobals.MainUIBGM.Play();
            {
                var i = 0;
                foreach (var item in pages)
                {
                    var ii = i;
                    foreach (var btn in item.Buttons)
                    {
                        btn.onClick.AddListener(() =>
                        {
                            SelectedPage = ii;
                        });
                    }
                    i++;
                }
            }
            foreach (var item in CloseButtons)
            {
                item.OnClick = () =>
                {
                    DialogManager.Show("Are you sure to exit?","", "Yes", () => { Application.Quit(); }, "No", null);
                };
            }
            //if(false)
            {
                //SettingsPageTabs.Init();
                //SettingsPageTabs.OnSelected = (i) =>
                //{
                //    foreach (var item in SettingsPages)
                //    {
                //        item.SetActive(false);
                //    }
                //    SettingsPages[i].SetActive(true);
                //};
            }
            //if (false)
            {
                for (int i = CampaignHolder.childCount - 1; i >= 0; i--)
                {
                    Destroy(CampaignHolder.GetChild(i).gameObject);
                }
                if (GameRuntime.CurrentGlobals.CurrentGameDef != null)
                    if (GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections != null)
                        if (GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections.Count > 0)
                            foreach (var item in GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections[0].MissionDefinitions)
                            {
                                var b = Instantiate(CampaignButton, CampaignHolder);
                                var cb = b.GetComponent<CampaignButton>();
                                cb.Init(group, item);
                            }
            }
            FirefightButton.OnClick = () => {
                if (SceneLoader.Instance.LoadScene("FIREFIGHTUI", true, false, false))
                {
                    //SceneLoader.Instance.HideScene(4);
                    GameRuntime.CurrentGlobals.MainUIBGM.Stop();
                }
                else
                {
                    DialogManager.Show("FIREFIGHT NOT ENABLED.", "FIREFIGHT FEATURE IS NOT ENABLED IN THIS VERSION OF SITE-13.", "OK", () => { });
                }
            };
            if(Parent!=null)
            Parent.RegisterRefresh(this);
            StartButton.OnClick = () => { StartCoroutine(TryLoadLevel()); };
            if (CustomizationButton != null)
            {
                CustomizationButton.OnClick = () => {
                    ParentManager.ShowPage(CustomizationPage);
                };
            }
            if(ForgeButton != null)
            {
                ForgeButton.OnClick = () => {
                    if (SceneLoader.Instance.LoadScene("FORGEUI", true, false, false))
                    {
                    }
                    else
                    {
                        DialogManager.Show("FORGE NOT ENABLED.", "FORGE FEATURE IS NOT ENABLED IN THIS VERSION OF SITE-13.", "OK", () => { });
                    }
                };
            }
            foreach (var item in ResetButtons)
            {
                item.OnClick = () => { };
            }
            //SettingsButton.onClick.AddListener(() => {
            //    this.GetComponent<CanvasGroup>().interactable=false;
            //    SceneManager.LoadScene("SettingsUI", LoadSceneMode.Additive);
            //    //SettingsPage.OnBack = () => {
            //    //    this. GetComponent<CanvasGroup>().interactable = true;
            //    //};
            //});
        }
        public IEnumerator WaitAndToSettings()
        {
            GlobalBlackCover.RequestShowCover(0.5f, 0.01f, 0.5f);
            yield return new WaitForSeconds(FadeDuration);

            SettingsController.Instance.Show(() => {
                GlobalBlackCover.RequestShowCover(0.5f, 0.01f, 0.5f);
            }, () => { 
                this.gameObject.SetActive(true);
            });
            this.gameObject.SetActive(false);
        }
        bool TryingLoadLevel = false;
        public IEnumerator TryLoadLevel()
        {
            if (TryingLoadLevel) yield break;
            TryingLoadLevel = true;
            GlobalBlackCover.RequestShowCover(1, 0.01f, 1f);
            yield return new WaitForSecondsRealtime(1);
            LoadLevel();
        }
        public void LoadLevel()
        {

            if (group.Selected != null)
            {
                GameRuntime.CurrentGlobals.CurrentMission = group.Selected.definition;
                GameRuntime.CurrentGlobals.MainUIBGM.Stop();
                Debugger.CurrentDebugger.Log("Enter mission:"+ GameRuntime.CurrentGlobals.CurrentMission.NameID+$"({GameRuntime.CurrentGlobals.CurrentMission.NameID})");
                SceneLoader.Instance.LoadScene(GameRuntime.CurrentGlobals.Scene_LevelLoader, true, true, false);
                SceneLoader.Instance.Unload(GameRuntime.CurrentGlobals.MainMenuSceneID);
            }
            else
            {
                DialogManager.Show("Select a mission", "Please select a mission to start", "OK", () => { });
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            {
                //Update Page Status.
                var _DeltaTime = DeltaTime * AnimationSpeed;
                for (int i = 0; i < pages.Count; i++)
                {
                    if (SelectedPage == i)
                    {
                        pages[i].Show(_DeltaTime);
                    }
                    else
                    {
                        pages[i].Hide(_DeltaTime);

                    }
                }
            }
        }
        //void HideAllPages()
        //{
        //    foreach (var item in pages)
        //    {
        //        item.Hide();
        //    }
        //}
    }
    [Serializable]
    public class ButtonedPage
    {
        public List<UIButton> Buttons;
        public CanvasGroup Page;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Hide(float DeltaTime)
        {
            if (Page.alpha > 0)
            {
                Page.alpha -= DeltaTime;
                //var d=1+1-Page.alpha;
                //Page.transform.localScale = new Vector3(d, d, d);

                var d = 1 - Page.alpha;
                d *= -400;
                Page.transform.localPosition = new Vector3(Page.transform.localPosition.x, Page.transform.localPosition.y, d);
            }
            else
            {
                if (Page.gameObject.activeSelf)
                    Page.gameObject.SetActive(false);
            }

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Show(float DeltaTime)
        {
            if (!Page.gameObject.activeSelf)
                Page.gameObject.SetActive(true);

            if (Page.alpha < 1)
            {
                Page.alpha += DeltaTime;
                var d = 1 - Page.alpha;
                d *= -400;
                Page.transform.localPosition = new Vector3(Page.transform.localPosition.x, Page.transform.localPosition.y, d);
            }
        }
    }

}