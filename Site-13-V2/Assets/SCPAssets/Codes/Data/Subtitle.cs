using TMPro;
using UnityEngine;

namespace Site13Kernel.Data
{
    public class Subtitle
    {
        public string ID;
        public string Fallback;
        public float Duration;
        public float CurrentTimeD;
        [HideInInspector]
        public TMP_Text ControlledSubtitle;
    }

}