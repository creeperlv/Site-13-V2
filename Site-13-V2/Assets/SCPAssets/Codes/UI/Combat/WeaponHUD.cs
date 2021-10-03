using Site13Kernel.Core;
using Site13Kernel.GameLogic.FPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.Combat
{
    public class WeaponHUD : ControlledBehavior
    {
        public Text DisplayText;
        public ProgressBar ProgressBar;
        public bool isPercentage;
        public bool isPrimary;
        public BasicWeapon ListeningWeapon;
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {

        }
    }
}
