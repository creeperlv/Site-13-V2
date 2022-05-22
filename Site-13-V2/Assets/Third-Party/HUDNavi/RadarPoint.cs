using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HUDNavi
{
    public class RadarPoint : MonoBehaviour
    {
        public Image RadarPointImage;
        public Color color;
        void Update()
        {
            if (RadarPointImage.color != color) RadarPointImage.color = color;
        }
    }
}
