using CLUNL.Localization;
using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class SubtitleController : ControlledBehavior
    {

        public Transform MainHolder;
        public Transform SubHolder;
        public TMP_Text MainTemplate;
        public TMP_Text SubTemplate;
        public void ShowSubtitle(Subtitle subtitle, bool isMain = true)
        {
            TMP_Text t;
            subtitle.Duration = Mathf.Max(subtitle.Duration, .5f);
            if (isMain)
            {
                t = Instantiate(MainTemplate.gameObject, MainHolder).GetComponent<TMP_Text>();

            }
            else
            {
                t = Instantiate(SubTemplate.gameObject, SubHolder).GetComponent<TMP_Text>();
            }
            t.text = Language.Find(subtitle.ID, subtitle.Fallback);
            subtitle.ControlledSubtitle = t.GetComponent<TMP_Text>();
            ShowedSubtitles.Add(subtitle);
        }
        [HideInInspector]
        public List<Subtitle> ShowedSubtitles=new List<Subtitle>();
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            foreach (var item in ShowedSubtitles)
            {
                item.CurrentTimeD += UnscaledDeltaTime;
                if (item.CurrentTimeD < .25f)
                {
                    item.ControlledSubtitle.alpha =
                        Mathf.Clamp(item.CurrentTimeD * 4f, 0f, 1f);
                }
                else if (item.CurrentTimeD > item.Duration - .25f)
                {
                    item.ControlledSubtitle.alpha =
                        Mathf.Clamp((item.Duration - item.CurrentTimeD) * 4f, 0f, 1f);
                }
            }
        }
        public override void Init()
        {
            if (GameRuntime.CurrentGlobals.SubtitleController != null)
            {
                Parent.UnregisterRefresh(this);
                Parent.UnregisterFixedRefresh(this);
                Destroy(this.gameObject);
                return;
            }
            GameRuntime.CurrentGlobals.SubtitleController = this;
            Parent.RegisterRefresh(this);
        }
    }
    public enum SubtitleType
    {
        MainSubtitle, Talks
    }

}