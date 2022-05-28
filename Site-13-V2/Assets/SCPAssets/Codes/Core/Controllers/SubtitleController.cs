using CLUNL.Localization;
using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.Core.Controllers
{
    public class SubtitleController : ControlledBehavior
    {

        public Transform MainHolder;
        public Transform SubHolder;
        public Text MainTemplate;
        public Text SubTemplate;
        public void ShowSubtitle(Subtitle subtitle, bool isMain = true)
        {
            Text t;
            subtitle.Duration = Mathf.Max(subtitle.Duration, .5f);
            if (isMain)
            {
                t = Instantiate(MainTemplate.gameObject, MainHolder).GetComponent<Text>();

            }
            else
            {
                t = Instantiate(SubTemplate.gameObject, SubHolder).GetComponent<Text>();
            }
            t.text =subtitle.Content;
            subtitle.ControlledSubtitle = t.GetComponent<Text>();
            ShowedSubtitles.Add(subtitle);
        }
        [HideInInspector]
        public List<Subtitle> ShowedSubtitles=new List<Subtitle>();
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            for (int i = ShowedSubtitles.Count-1; i >=0; i--)
            {
                var item = ShowedSubtitles[i];

                item.CurrentTimeD += UnscaledDeltaTime;
                if (item.CurrentTimeD < .25f)
                {
                    var c = item.ControlledSubtitle.color; c.a =
                        Mathf.Clamp(item.CurrentTimeD * 4f, 0f, 1f);
                    item.ControlledSubtitle.color = c;
                }
                else if (item.CurrentTimeD > item.Duration - .25f)
                {
                    var c = item.ControlledSubtitle.color; c.a =
                        Mathf.Clamp((item.Duration - item.CurrentTimeD) * 4f, 0f, 1f);
                    item.ControlledSubtitle.color = c;
                    //item.ControlledSubtitle.alpha =
                    //    Mathf.Clamp((item.Duration - item.CurrentTimeD) * 4f, 0f, 1f);
                }
                if (item.CurrentTimeD > item.Duration)
                {
                    ShowedSubtitles.Remove(item);
                    Destroy(item.ControlledSubtitle.gameObject);
                }
            }
            //foreach (var item in ShowedSubtitles)
            //{
            //}
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