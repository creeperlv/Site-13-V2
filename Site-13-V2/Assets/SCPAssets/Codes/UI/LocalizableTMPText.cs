using CLUNL.Localization;
using Site13Kernel.GameLogic.RuntimeScenes;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.UI
{
    public class LocalizableTMPText : MonoBehaviour
    {
        public TMPro.TMP_Text ControlledText;
        public LocalizedString localizedString;
        public List<LRTRegistryItem> Parameters;
        public void Start()
        {
            ApplyText();
        }
        private void Awake()
        {
            ApplyText();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyText()
        {

            if (Parameters.Count != 0)
            {
                ControlledText.text = string.Format(localizedString, Parameters.ToArray());
            }
            else
            {
                ControlledText.text = localizedString;
            }
        }
    }
}
