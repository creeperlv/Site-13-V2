using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Customization
{
    public class WeaponCoatingPalettes : MonoBehaviour
    {
        public static WeaponCoatingPalettes Instance;
        public List<WeaponCustomizationPalette> Palettes;
        void Start()
        {
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
