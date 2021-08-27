using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class SubtitleController : ControlledBehavior
    {
        public Transform Holder;
        public TMPro.TMP_Text Template;
        public void ShowSubtitle(Subtitle subtitle)
        {
            var t=GameObject.Instantiate(Template.gameObject, Holder);
            
        }
        public List<Subtitle> ShowedSubtitles;
        private void FixedUpdate()
        {

        }
        public override void Init()
        {
        }
    }
    public class Subtitle
    {
        public string ID;
        public string Fallback;
        public float Duration;
        public float CurrentTimeD;
        [HideInInspector]
        public TMPro.TMP_Text ControlledSubtitle;
    }

}