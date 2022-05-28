using CLUNL.Localization;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.Data
{
    [Serializable]
    public class Subtitle
    {
        public LocalizedString Content;
        public float Duration;
        public float CurrentTimeD;
        [HideInInspector]
        public Text ControlledSubtitle;
    }

}