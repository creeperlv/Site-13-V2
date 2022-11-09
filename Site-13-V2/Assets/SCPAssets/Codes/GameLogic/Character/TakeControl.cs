using CLUNL.Localization;
using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using Site13Kernel.GameLogic.Controls;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.UI.HUD;
using System;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Site13Kernel.GameLogic.Character
{
    [Serializable]
    public class TakeControl : ControlledBehavior
    {
        public static TakeControl Instance;
        public BipedController controller;
        public ActiveInteractor Interactor;
        public BipedEntity entity;
        public Action OnWeaponChange = null;
        public float KillResetTime = 5;
        public void OnEnable()
        {
            Instance = this;
            controller = GetComponentInChildren<BipedController>();
            entity = GetComponentInChildren<BipedEntity>();
            entity.isTookControl = true;
            entity.EntityBag.OnObtainWeapon.Add(InvokeWeaponChange);
            entity.EntityBag.OnDropWeapon.Add(InvokeWeaponChange);
            entity.OnSwapWeapon.Add(WeaponChange);
            entity.OnCauseDamage.Add(CauseDamage);
            Interactor = GetComponent<ActiveInteractor>();
        }
        int KillAmount;
        float KillCountReset = 0;
        public void Update()
        {
            float UDT = Time.unscaledDeltaTime;
            UPD(UDT);
        }
        public void UPD(float UDT)
        {
            if (KillCountReset < 0) return;
            KillCountReset -= UDT;
            if (KillCountReset < 0)
            {
                KillAmount = 0;
            }
        }
        void CauseDamage(DamagableEntity de, DamageInformation Info, bool isDied)
        {
            if (isDied)
            {
                if (de == entity)
                {
                    MedalContainer.Instance.NewScoreAccount(-100);
                    return;
                }
                MedalContainer.Instance.NewScoreAccount(100);
                KillAmount++;
                KillCountReset = KillResetTime;
                if (KillAmount >= 2)
                {
                    MedalContainer.Instance.NewMedal(KillAmount - 1);
                }
            }
        }
        void InvokeWeaponChange(GenericWeapon gw)
        {
            WeaponChange();
            if (Interactor.Interactive is Pickupable p)
            {
                if (p.AssociatedGenericWeapon.WeaponData.WeaponID == gw.WeaponData.WeaponID)
                {
                    Interactor.__hint = true;
                    Interactor.Hint = new LocalizedString(string.Empty, string.Empty);
                }
            }
            if (OnWeaponChange != null) { OnWeaponChange(); }
        }
        void WeaponChange()
        {
            for (int i = 0; i < entity.EntityBag.Weapons.Count; i++)
            {
                var TargetWeapon = entity.EntityBag.Weapons[i].WeaponData;
                var TargetHUD = HUDBase.Instance.WeaponHUDs[i];
                if (WeaponPool.CurrentPool.WeaponItemMap.ContainsKey(TargetWeapon.WeaponID))
                {
                    TargetHUD.DisplayTextTitle.text = Language.Find(TargetWeapon.WeaponID + ".DispName",
                        WeaponPool.CurrentPool.WeaponItemMap[TargetWeapon.WeaponID].NameFallback);
                    TargetHUD.IconImg.sprite = WeaponPool.CurrentPool.WeaponItemMap[TargetWeapon.WeaponID].WeaponIcon;
                    if (WeaponPool.CurrentPool.WeaponItemMap[TargetWeapon.WeaponID].WeaponMaterial != null)
                        TargetHUD.IconImg.material = WeaponPool.CurrentPool.WeaponItemMap[TargetWeapon.WeaponID].WeaponMaterial;

                }
                else
                {
                    Debugger.CurrentDebugger.LogError($"{TargetWeapon.WeaponID} does not exists in the weapon map!");
                }
            }

        }
        private void OnDisable()
        {
            StopTakeControl();
            entity.EntityBag.OnObtainWeapon.Remove(InvokeWeaponChange);
            entity.EntityBag.OnDropWeapon.Remove(InvokeWeaponChange);
            entity.OnSwapWeapon.Remove(WeaponChange);
        }
        public void StopTakeControl()
        {
            entity.isTookControl = false;

        }
    }
}
