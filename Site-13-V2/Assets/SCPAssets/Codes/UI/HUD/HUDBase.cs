using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.GameLogic;
using Site13Kernel.GameLogic.Character;
using Site13Kernel.GameLogic.Controls;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.GameLogic.Level;
using Site13Kernel.UI.Combat;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.HUD
{
    public class HUDBase : ControlledBehavior
    {
        public static HUDBase Instance;
        public bool UseBipedEntity = false;
        public ProgressBar HP;
        public List<ProgressBar> Shield;

        public Animator OnShieldDown;

        public WeaponHUD W_HUD0;
        public WeaponHUD W_HUD1;
        public KVList<int, GrenadeHUD> GrenadeHUD;
        public Dictionary<int, GrenadeHUD> __GrenadeHUD;
        public GrenadeHUD G_HUD0;
        public GrenadeHUD G_HUD1;

        public KVList<int, CrosshairContainer> _Crosshairs = new KVList<int, CrosshairContainer>();
        public KVList<int, CrosshairContainer> _ZoomOverlays = new KVList<int, CrosshairContainer>();
        public Dictionary<int, CrosshairContainer> Crosshairs = new Dictionary<int, CrosshairContainer>();
        public Dictionary<int, CrosshairContainer> ZoomOverlays = new Dictionary<int, CrosshairContainer>();

        public List<WeaponHUD> WeaponHUDs = new List<WeaponHUD>();

        public Vector2 W_HUD_PrimaryPosition;
        public Vector3 W_HUD_PrimaryScale;
        public Vector2 W_HUD_SecondaryPosition;
        public Vector3 W_HUD_SecondaryScale;

        public Image E_HUD_ICON;
        public Text E_HUD_COUNT;
        public ProgressBar HeatIndicator;
        public TextMeshProUGUI IteractHint;

        public CanvasGroup TotalHUD;
        public bool Show;
        public float ShowSpeed = 1;
        public float ZoomOverlaySpeed = 1;
        public float ShowThreshold = 0.01f;
        public float ToggleScale = 0.5f;
        public Transform IndicatorHolder;
        public PrefabReference Indicator;
        public PrefabReference KillIndicator;
        public GameObject OnShow;
        int LastWeaponID = -1;
        int LastZoomID = -1;
        bool __s;
        public override void Init()
        {
            Instance = this;
            Crosshairs = _Crosshairs.ObtainMap();
            ZoomOverlays = _ZoomOverlays.ObtainMap();
            foreach (var item in Crosshairs)
            {
                item.Value.Init();
            }
            for (int i = 0; i < WeaponHUDs.Count; i++)
            {
                var item = WeaponHUDs[i];
                item.IndexInStack = i;
            }
            __GrenadeHUD = GrenadeHUD.ObtainMap();
        }
        public void TryIndicateAKill()
        {
            var PREFAB = ResourceBuilder.ObtainGameObject(KillIndicator.ID);
            if (PREFAB != null)
            {
                var effect = EffectController.CurrentEffectController.Spawn(KillIndicator, Vector3.zero, Quaternion.identity, Vector3.one, IndicatorHolder);
                var RT = effect.transform as RectTransform;
                RT.anchoredPosition3D = Vector3.zero;
                RT.localRotation = Quaternion.identity;

            }
            else
            {
            }
        }
        public void TryIndicateAHit()
        {
            var PREFAB = ResourceBuilder.ObtainGameObject(Indicator.ID);
            if (PREFAB != null)
            {
                var effect = EffectController.CurrentEffectController.Spawn(Indicator, Vector3.zero, Quaternion.identity, Vector3.one, IndicatorHolder);
                var RT = effect.transform as RectTransform;
                RT.anchoredPosition3D = Vector3.zero;
                RT.localRotation = Quaternion.identity;

            }
            else
            {
            }
        }
        public override void Refresh(float DT, float UDT)
        {
            bool _s = Show;
            if (UseBipedEntity)
            {

                _s = _s & TakeControl.Instance != null;
            }
            if (__s != _s)
            {
                __s = _s;
                if (OnShow != null)
                {
                    if (__s)
                    {
                        OnShow.SetActive(true);
                    }
                    else
                        OnShow.SetActive(false);
                }
            }
            if (_s)
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
            if (TotalHUD.alpha == 0) return;
            {
                BioEntity entity = null;
                if (UseBipedEntity)
                {
                    if (TakeControl.Instance != null)
                        entity = TakeControl.Instance.entity;
                }
                else
                {
                    if (FPSController.Instance != null)
                    {
                        entity = FPSController.Instance.CurrentEntity;
                    }
                }
                if (entity != null)
                {
                    if (HP != null)
                    {
                        HP.Value = entity.CurrentHP;
                        HP.MaxValue = entity.MaxHP;
                    }
                    if (entity.CurrentShield == 0 && entity.MaxShield != 0)
                    {
                        if (OnShieldDown != null)
                        {
                            if (!OnShieldDown.gameObject.activeSelf)
                            {
                                OnShieldDown.gameObject.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        if (OnShieldDown != null)
                        {
                            if (OnShieldDown.gameObject.activeSelf)
                            {
                                OnShieldDown.gameObject.SetActive(false);
                            }

                        }
                    }
                    if (Shield.Count > 0)
                    {
                        foreach (var item in Shield)
                        {
                            item.Value = entity.CurrentShield;
                            item.MaxValue = entity.MaxShield;
                        }
                    }
                    {
                        /////////////////////////
                        // Biped Implementation//
                        /////////////////////////
                        if (UseBipedEntity)
                        {
                            {
                                // HUD Hint.
                                var interactor = TakeControl.Instance.Interactor;
                                if (interactor.__hint)
                                {
                                    IteractHint.text = interactor.Hint.ToString();
                                    interactor.__hint = false;
                                }
                            }
                            var _entity = TakeControl.Instance.entity;
                            {
                                //Equipments
                                if (E_HUD_COUNT != null)
                                {
                                    if (_entity.EntityBag.Equipments.TryGetValue(_entity.EntityBag.CurrentEquipment, out var i))
                                    {
                                        E_HUD_COUNT.text = i.ToString();
                                    }
                                    else E_HUD_COUNT.text = "0";
                                }
                                if (E_HUD_ICON != null)
                                {
                                    if (_entity.EntityBag.LastSelectedEquipment != _entity.EntityBag.CurrentEquipment)
                                    {
                                        _entity.EntityBag.LastSelectedEquipment = _entity.EntityBag.CurrentEquipment;
                                        if (EquipmentManifest.Instance.EqupimentMap.TryGetValue(_entity.EntityBag.CurrentEquipment, out var def))
                                        {
                                            E_HUD_ICON.sprite = def.Icon;
                                            E_HUD_ICON.material = def.IconMat;
                                        }
                                    }
                                }
                            }
                            {
                                // Crosshair , Zoom and Heat
                                int TargetCrosshair;
                                int TargetZoom;
                                if (_entity.EntityBag.Weapons.Count > 0)
                                {
                                    TargetCrosshair = _entity.EntityBag.Weapons[_entity.EntityBag.CurrentWeapon].CrossHairID;
                                    TargetZoom = _entity.EntityBag.Weapons[_entity.EntityBag.CurrentWeapon].ZoomID;
                                    HeatIndicator.Value = _entity.EntityBag.Weapons[_entity.EntityBag.CurrentWeapon].WeaponData.CurrentHeat;
                                }
                                else
                                {
                                    TargetCrosshair = 0;
                                    TargetZoom = 0;
                                }
                                var cc = Crosshairs[TargetCrosshair];
                                var zoom = ZoomOverlays[TargetZoom];
                                {
                                    if (TakeControl.Instance.controller.isInRange)
                                    {
                                        cc.SetColor(Color.red);
                                    }
                                    else
                                    {
                                        cc.SetColor(Color.white);
                                    }
                                }
                                if (_entity.EntityBag.Weapons.Count > 0)
                                {
                                    var Weapon = _entity.EntityBag.Weapons[_entity.EntityBag.CurrentWeapon];
                                    cc.UpdateCrosshair(Weapon.Recoil);
                                    if (TargetZoom != 0)
                                        zoom.UpdateCrosshair(Weapon.Recoil);
                                    if (Weapon.AimingMode == 1)
                                    {
                                        if (zoom.canvas.alpha != 1)
                                        {
                                            zoom.canvas.alpha = MathUtilities.SmoothClose(zoom.canvas.alpha, 1, DT * ShowSpeed);
                                            if (1 - TotalHUD.alpha < ShowThreshold)
                                            {
                                                TotalHUD.alpha = 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (zoom.canvas.alpha != 0)
                                        {
                                            zoom.canvas.alpha = MathUtilities.SmoothClose(zoom.canvas.alpha, 0, DT * ShowSpeed);
                                            if (zoom.canvas.alpha < ShowThreshold)
                                            {
                                                zoom.canvas.alpha = 0;
                                            }
                                        }
                                    }
                                }
                                else
                                {

                                }

                                if (LastWeaponID != TargetCrosshair)
                                {
                                    ShowCrosshair(TargetCrosshair);
                                    LastWeaponID = TargetCrosshair;
                                }
                                if (LastZoomID != TargetZoom)
                                {
                                    ShowZoom(TargetZoom);
                                    LastZoomID = TargetZoom;
                                }
                            }
                            {
                                //Weapon Binding
                                if (_entity.EntityBag.Weapons.Count >= 1)
                                    W_HUD0._ListeningWeapon = _entity.EntityBag.Weapons[0];
                                else W_HUD0._ListeningWeapon = null;
                                W_HUD0.isPrimary = 0 == _entity.EntityBag.CurrentWeapon;
                                if (_entity.EntityBag.Weapons.Count >= 2)
                                    W_HUD1._ListeningWeapon = _entity.EntityBag.Weapons[1];
                                else W_HUD1._ListeningWeapon = null;
                                W_HUD1.isPrimary = 1 == _entity.EntityBag.CurrentWeapon;
                            }
                            foreach (var item in WeaponHUDs)
                            {
                                if (item.IndexInStack == _entity.EntityBag.CurrentWeapon || (_entity.EntityBag.CurrentWeapon != _entity.EntityBag.Weapons.Count - 1 & item.IndexInStack + 1 == _entity.EntityBag.CurrentWeapon) || (_entity.EntityBag.CurrentWeapon == _entity.EntityBag.Weapons.Count - 1 & item.IndexInStack == 0))
                                {
                                    if (item.gameObject.activeSelf == false)
                                        item.gameObject.SetActive(true);
                                }
                                else
                                {
                                    if (item.gameObject.activeSelf == true)
                                        item.gameObject.SetActive(false);
                                }
                                item.Refresh(DT, UDT);
                            }
                            foreach (var item in __GrenadeHUD)
                            {
                                if (_entity.EntityBag.Grenades.TryGetValue(item.Key, out var g))
                                {
                                    item.Value.UpdateValue(g, item.Key == _entity.EntityBag.CurrentGrenade);
                                }
                                else
                                    item.Value.UpdateValue(null, item.Key == _entity.EntityBag.CurrentGrenade);
                            }
                            //foreach (var item in _entity.EntityBag.Grenades)
                            //{
                            //    __GrenadeHUD[item.Key].UpdateValue(item.Value, item.Key == _entity.EntityBag.CurrentGrenade);
                            //}
                        }
                        else
                        {

                            W_HUD0.Refresh(DT, UDT);
                            W_HUD1.Refresh(DT, UDT);
                        }
                    }
                }
                G_HUD0.Refresh(DT, UDT);
                G_HUD1.Refresh(DT, UDT);
            }
        }
        public void ShowZoom(int index)
        {
            foreach (var item in ZoomOverlays)
            {
                item.Value.Hide();
            }
            ZoomOverlays[index].Show();
        }
        [Header("MissionHint")]
        public GameObject MissionHint_HintObject;
        public Text MissionHint_TextHolder;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IssueMission(string Text)
        {
            MissionHint_TextHolder.text = Text;
            StartCoroutine(ShowMission());
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator ShowMission()
        {
            MissionHint_HintObject.SetActive(false);
            yield return null;
            MissionHint_HintObject.SetActive(true);
        }
        public void ShowCrosshair(int index)
        {
            foreach (var item in Crosshairs)
            {
                item.Value.Hide();
            }
            Crosshairs[index].Show();
        }
    }
}
