using CLUNL.Localization;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.Customization;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.Customizations
{
    public class WeaponCustomizationMenu : Page
    {
        public UIButton BackButton;
        public GameObject ButtonPrefab;
        public Transform WeaponHolder;
        public Transform WeaponDemo;
        public Transform CoatingsHolder;
        public Text DescriptionText;
        public List<WeaponCoatingCollection> Weapons;
        // Start is called before the first frame update
        public override void Init()
        {
            BackButton.OnClick = () =>
            {
                ParentManager.ShowPage(0);
            };
            foreach (var item in Weapons)
            {
                var BTN = GameObject.Instantiate(ButtonPrefab, WeaponHolder);
                var UIBtn = BTN.GetComponent<UIButton>();
                if (UIBtn != null)
                {
                    UIBtn.Content = item.DisplayName;
                    UIBtn.OnClick = () =>
                    {
                        string SelectedCoating="";
                        {
                            if (WeaponDemo.childCount > 0)
                                Destroy(WeaponDemo.GetChild(0).gameObject);
                            var weapon = ObjectGenerator.Instantiate(item.Weapon, WeaponDemo);
                            {

                                var CW = weapon.GetComponentInChildren<CustomizableWeapon>();
                                CW.TargetWeaponCoating = PlayerWeaponCoatings.CurrentPlayerWeaponCoatings.GetCoating(CW.WeaponID, "STANDARD");
                                SelectedCoating = CW.TargetWeaponCoating;
                                CW.ApplyCoating();
                            }
                            var r = weapon.GetComponentsInChildren<Rigidbody>();
                            foreach (var _r in r)
                            {
                                Destroy(_r);
                            }
                        }
                        for (int i = CoatingsHolder.childCount - 1; i >= 0; i--)
                        {
                            Destroy(CoatingsHolder.GetChild(i).gameObject);
                        }
                        foreach (var coating in item.Coatings)
                        {
                            if(coating.ID== SelectedCoating)
                            {
                                DescriptionText.text = coating.Description;
                            }
                            var BTN2 = GameObject.Instantiate(ButtonPrefab, CoatingsHolder);
                            var UIBtn2 = BTN2.GetComponent<UIButton>();
                            if (UIBtn2 != null)
                            {
                                UIBtn2.Content = coating.Name;
                                UIBtn2.OnClick = () =>
                                {
                                    DescriptionText.text = coating.Description;
                                    if (WeaponDemo.childCount > 0)
                                    {
                                        var t = WeaponDemo.GetChild(0);
                                        var CW = t.gameObject.GetComponentInChildren<CustomizableWeapon>();
                                        CW.TargetWeaponCoating = coating.ID;
                                        CW.ApplyCoating();
                                        PlayerWeaponCoatings.CurrentPlayerWeaponCoatings.SetCoating(CW.WeaponID, CW.TargetWeaponCoating);
                                    }
                                };
                            }
                        }
                    };
                }
            }
            {
                var button = WeaponHolder.GetChild(0).GetComponent<UIButton>();
                if (button != null)
                    button.OnClick();
            }
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {

        }
    }
    [Serializable]
    public class WeaponCoatingCollection
    {
        public PrefabReference Weapon;
        public LocalizedString DisplayName;
        public List<WeaponCoating> Coatings = new List<WeaponCoating>();
    }

}
