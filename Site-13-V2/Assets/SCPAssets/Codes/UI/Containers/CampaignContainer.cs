using Site13Kernel.Core;
using Site13Kernel.UI;
using Site13Kernel.UI.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CLUNL.Localization;
using ToggleButton = Site13Kernel.UI.Elements.ToggleButton;
using Site13Kernel.Data;

namespace Site13Kernel.UI.Containers
{
    public class CampaignContainer : MonoBehaviour
    {
        public Transform LevelContainer;
        public GameObject PrimitiveInteractivable;
        public Animator CampaignDetailAnimator;
        public string ShowAnimation;
        public float ShowT;
        public Image CoverContainer;
        public bool initOnStart = true;
        public Text Title;
        public Text Description;
        public RadioButtonGroupOverToggleButton group = new RadioButtonGroupOverToggleButton();
        void Start()
        {
            if (initOnStart) __init();
        }
        public IEnumerator __0(MissionDefinition item)
        {
            var title = new LocalizedString(item.NameID + ".Name", item.DispFallback);
            var description = new LocalizedString(item.NameID + ".Desc", item.DescFallback);
            CampaignDetailAnimator.Play(ShowAnimation,-1,0);
            //CampaignDetailAnimator.playbackTime = 0;
            //CampaignDetailAnimator.StartPlayback();
            yield return new WaitForSecondsRealtime(ShowT);
            Title.text = title;
            Description.text = description;

            try
            {
                CoverContainer.sprite = GameRuntime.CurrentGlobals.CurrentGameDef.Sprites[item.ImageName].LoadedSprite;
            }
            catch (Exception)
            {
            }
        }
        bool isLoadDone;
        public void __init()
        {
            if (GameRuntime.CurrentGlobals.CurrentGameDef != null)
                if (GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections != null)
                    if (GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections.Count > 0)
                    {
                        foreach (var item in GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections[0].MissionDefinitions)
                        {
                            var title = new LocalizedString(item.NameID + ".Name", item.DispFallback);
                            var description = new LocalizedString(item.NameID + ".Desc", item.DescFallback);
                            var b = Instantiate(PrimitiveInteractivable, LevelContainer);
                            var cb = b.GetComponent<Elements.ToggleButton>();
                            group.RegisterButton(cb);
                            cb.SetText(title);
                            cb.PreventUncheckOnClick = true;
                            cb.Checked += () =>
                            {
                                if (!isLoadDone)
                                {
                                    Title.text = title;
                                    Description.text = description;
                                    try
                                    {
                                        CoverContainer.sprite = GameRuntime.CurrentGlobals.CurrentGameDef.Sprites[item.ImageName].LoadedSprite;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                else
                                StartCoroutine(__0(item));
                            };
                        }
                    }
            if(group.FirstButton(out var tb))
            {
                tb.isOn = true;
            }
            isLoadDone = true;
        }
    }
}
