using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.UI.Combat;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.HUD
{
    public class HUDBase : ControlledBehavior
    {
        public static HUDBase Instance;
        public ProgressBar HP;
        public List<ProgressBar> Shield;
        public WeaponHUD W_HUD0;
        public WeaponHUD W_HUD1;
        public KVList<int, GrenadeHUD> GrenadeHUD;
        public Dictionary<int, GrenadeHUD> __GrenadeHUD;
        public GrenadeHUD G_HUD0;
        public GrenadeHUD G_HUD1;

        public Vector2 W_HUD_PrimaryPosition;
        public Vector3 W_HUD_PrimaryScale;
        public Vector2 W_HUD_SecondaryPosition;
        public Vector3 W_HUD_SecondaryScale;

        public Image E_HUD_ICON;
        public Text E_HUD_COUNT;

        public CanvasGroup TotalHUD;
        public bool Show;
        public float ShowSpeed = 1;
        public float ShowThreshold = 0.01f;
        public float ToggleScale = 0.5f;
        public override void Init()
        {
            Instance = this;
            __GrenadeHUD = GrenadeHUD.ObtainMap();
        }
        public override void Refresh(float DT, float UDT)
        {
            if (Show)
            {
                if (TotalHUD.alpha != 1)
                {
                    TotalHUD.alpha = MathUtilities.SmoothClose(TotalHUD.alpha, 1, DT * ShowSpeed);
                    TotalHUD.transform.localScale = Vector3.one * (1 + (1 - TotalHUD.alpha) * ToggleScale);
                    if (1 - TotalHUD.alpha < ShowThreshold)
                    {
                        TotalHUD.alpha = 1;
                    }
                }
            }
            else
            {
                if (TotalHUD.alpha != 0)
                {
                    TotalHUD.alpha = MathUtilities.SmoothClose(TotalHUD.alpha, 0, DT * ShowSpeed);
                    TotalHUD.transform.localScale = Vector3.one * (1 + (1 - TotalHUD.alpha) * ToggleScale);
                    if (TotalHUD.alpha < ShowThreshold)
                    {
                        TotalHUD.alpha = 0;
                    }
                }
            }
            {

                if (FPSController.Instance != null)
                {

                    if (HP != null)
                    {
                        HP.Value = FPSController.Instance.CurrentEntity.CurrentHP;
                        HP.MaxValue = FPSController.Instance.CurrentEntity.MaxHP;
                    }
                    if (Shield.Count > 0)
                    {
                        foreach (var item in Shield)
                        {
                            item.Value = FPSController.Instance.CurrentEntity.CurrentShield;
                            item.MaxValue = FPSController.Instance.CurrentEntity.MaxShield;
                        }
                    }
                }
                W_HUD0.Refresh(DT, UDT);
                W_HUD1.Refresh(DT, UDT);
                G_HUD0.Refresh(DT, UDT);
                G_HUD1.Refresh(DT, UDT);
            }
        }
    }
}
