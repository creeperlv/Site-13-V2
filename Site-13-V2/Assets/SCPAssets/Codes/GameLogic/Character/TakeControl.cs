using CLUNL.Localization;
using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using Site13Kernel.GameLogic.Controls;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.UI.HUD;
using System;
using UnityEditor;

namespace Site13Kernel.GameLogic.Character
{
    [Serializable]
    public class TakeControl : ControlledBehavior
    {
        public static TakeControl Instance;
        public BipedController controller;
        public ActiveInteractor Interactor;
        public BipedEntity entity;
        public Action OnWeaponChange=null;
        public void OnEnable()
        {
            Instance = this;
            controller = GetComponentInChildren<BipedController>();
            entity = GetComponentInChildren<BipedEntity>();
            entity.isTookControl= true;
            entity.EntityBag.OnObtainWeapon.Add(InvokeWeaponChange);
            entity.EntityBag.OnDropWeapon.Add(InvokeWeaponChange);
            entity.OnSwapWeapon.Add(WeaponChange);
            Interactor = GetComponent<ActiveInteractor>();
        }
        void InvokeWeaponChange(GenericWeapon gw)
        {
            WeaponChange();
            if(Interactor.Interactive is Pickupable p)
            {
                if (p.AssociatedGenericWeapon == gw)
                {
                    Interactor.SwapInteractive(null);
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
            entity.isTookControl= false;

        }
    }
}
