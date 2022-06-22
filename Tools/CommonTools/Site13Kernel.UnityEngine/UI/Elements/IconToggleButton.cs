using System;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.Elements
{
    [Serializable]
    [AddComponentMenu("UI/Site13/IconToggleButton")]
    public class IconToggleButton:ToggleButton
    {
        [SerializeField]
        public Image ControlledImage;
    }
}
