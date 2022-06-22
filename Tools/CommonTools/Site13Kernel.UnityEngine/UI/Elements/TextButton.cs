using CLUNL.Localization;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.Elements
{
    [Serializable]
    [AddComponentMenu("UI/Site13/TextButton")]
    public class TextButton : Button {
        [SerializeField]
        public Text ControlledText;
        public void SetText(LocalizedString Content)
        {
            if (ControlledText != null) ControlledText.text = Content;
        }
    }
}
