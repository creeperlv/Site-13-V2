using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Customization
{
    public class CustomizableWeapon : MonoBehaviour
    {
        public string TargetWeaponCoating = "DEFAULT";
        public string WeaponID;
        public KVList<int, CustomizableMeshRenderer> ControlledCustomizableMeshRenders;
        public void ApplyCoating()
        {

        }
    }
}
