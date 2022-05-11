using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.GameLogic;
using Site13Kernel.GameLogic.FPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.Combat
{
    public class WeaponHUD : ControlledBehavior
    {
        public Image IconImg;
        public Text DisplayText;
        public Text DisplayTextTitle;
        public ProgressBar ProgressBar;
        public bool isPercentage;
        public bool isPrimary;
        public ControlledWeapon ListeningWeapon;
        public FPSController Holder;
        public float HUDMoveSpeed;
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (ListeningWeapon != null)
            {

                if (!this.gameObject.activeSelf)
                    this.gameObject.SetActive(true);
                if (!isPercentage)
                {
                    DisplayText.text = $"{ListeningWeapon.Weapon.Base.CurrentMagazine}|{ListeningWeapon.Weapon.Base.CurrentBackup}";
                    if (!DisplayText.gameObject.activeSelf)
                    {
                        DisplayText.gameObject.SetActive(true);
                    }
                    if (ProgressBar.gameObject.activeSelf)
                    {
                        ProgressBar.gameObject.SetActive(false); 
                    }
                }
                else
                {
                    ProgressBar.Value= ListeningWeapon.Weapon.Base.CurrentMagazine/ ListeningWeapon.Weapon.Base.MagazineCapacity; 
                    if (DisplayText.gameObject.activeSelf)
                    {
                        DisplayText.gameObject.SetActive(false);
                    }
                    if (!ProgressBar.gameObject.activeSelf)
                    {
                        ProgressBar.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (this.gameObject.activeSelf)
                    this.gameObject.SetActive(false);
            }
            var t = (transform as RectTransform);
            if (isPrimary)
            {
                Move(t, Holder.W_HUD_PrimaryPosition, Holder.W_HUD_PrimaryScale, DeltaTime);
            }
            else
            {
                Move(t, Holder.W_HUD_SecondaryPosition, Holder.W_HUD_SecondaryScale, DeltaTime);
            }
        }
        public void Move(RectTransform t, Vector2 TargetPosition, Vector3 TargetScale, float DT)
        {
            {
                var D = TargetPosition - t.anchoredPosition;
                t.anchoredPosition += D * DT * HUDMoveSpeed;

            }
            {
                var D = TargetScale - t.localScale;
                t.localScale += D * DT * HUDMoveSpeed;
            }
        }
    }
}
