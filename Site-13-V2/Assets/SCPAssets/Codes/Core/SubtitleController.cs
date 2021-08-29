using CLUNL.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Site13Kernel.Core
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
            if (isMain)
            {
                t = Instantiate(MainTemplate.gameObject, MainHolder).GetComponent<TMP_Text>();

            }
            else
            {
                t = Instantiate(MainTemplate.gameObject, MainHolder).GetComponent<TMP_Text>();
            }
            t.text = Language.Find(subtitle.ID, subtitle.Fallback);
            subtitle.ControlledSubtitle = t.GetComponent<TMP_Text>();
        }
        [HideInInspector]
        public List<Subtitle> ShowedSubtitles;
        private void FixedUpdate()
        {
            foreach (var item in ShowedSubtitles)
            {

            }
        }
        public override void Init()
        {
            GameRuntime.CurrentGlobals.SubtitleController = this;
        }
    }
    public class Subtitle
    {
        public string ID;
        public string Fallback;
        public float Duration;
        public float CurrentTimeD;
        [HideInInspector]
        public TMP_Text ControlledSubtitle;
    }
    public enum SubtitleType
    {
        MainSubtitle, Talks
    }

}