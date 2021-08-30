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
        int SelectedPage=0;
        public float AnimationSpeed=4;
        public ButtonGroup SettingsPageTabs;
        public List<GameObject> SettingsPages;
        public override void Init()
        {
            {
                SettingsPageTabs.Init();
                SettingsPageTabs.OnSelected = (i) => {
                    foreach (var item in SettingsPages)
                    {
                        item.SetActive(false);
                    }
                    SettingsPages[i].SetActive(true);
                };
            }
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
            {
                var i=0;
                foreach (var item in pages)
                {
                    var ii=i;
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
            Parent.RegisterRefresh(this);

            //SettingsButton.onClick.AddListener(() => {
            //    this.GetComponent<CanvasGroup>().interactable=false;
            //    SceneManager.LoadScene("SettingsUI", LoadSceneMode.Additive);
            //    //SettingsPage.OnBack = () => {
            //    //    this. GetComponent<CanvasGroup>().interactable = true;
            //    //};
            //});
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            {
                //Update Page Status.
                var  _DeltaTime =DeltaTime* AnimationSpeed;
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
        public void Hide(float DeltaTime)
        {
            if (Page.alpha > 0)
            {
                Page.alpha -= DeltaTime;
                //var d=1+1-Page.alpha;
                //Page.transform.localScale = new Vector3(d, d, d);

                var d=1-Page.alpha;
                d *= -400;
                Page.transform.localPosition = new Vector3(Page.transform.localPosition.x, Page.transform.localPosition.y, d);
            }
            else
            {
                if (Page.gameObject.activeSelf)
                    Page.gameObject.SetActive(false);
            }

        }
        public void Show(float DeltaTime)
        {
            if (!Page.gameObject.activeSelf)
                Page.gameObject.SetActive(true);

            if (Page.alpha < 1)
            {
                Page.alpha += DeltaTime;
                var d=1-Page.alpha;
                d *= -400;
                Page.transform.localPosition = new Vector3(Page.transform.localPosition.x, Page.transform.localPosition.y, d);
            }
        }
    }

}