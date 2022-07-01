using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.FPS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.GameLogic
{
    public class ControlledWeapon : ControlledBehavior
    {
        public BasicWeapon Weapon;
        [Header("Zoom")]
        public bool CanZoom = false;
        public CanvasGroup ZoomHUD = null;
        public float ZoomFov = 50;
        public Transform ZoomEffectPoint;
        [Header("Movement")]
        public Vector3 NormalPosition;
        public Vector3 RunningPosition;
        public Vector3 NormalRotationEuler;
        public Vector3 RunningRotationEuler;
        public List<ControlledCrosshair> Crosshairs;
        public GameObject HUDCanvas;
        [Header("HUD Related")]
        public bool isPercentage;
        public GameObject CrosshairCanvasGroup;
        public Renderer FPSRenderer;
        public AudioSource ZoomInEffect = null;
        public AudioSource ZoomOutEffect = null;
        public Image HeatBar = null;
        public byte ZoomInEffectState = 0;
        public byte ZoomOutEffectState = 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Combat()
        {
            Weapon.Combat();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fire()
        {
            Weapon.Fire();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Press()
        {
            Weapon.Press();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Reload()
        {
            return Weapon.Reload();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unfire()
        {
            Weapon.Unfire();
        }
        bool isInited = false;
        public override void Init()
        {
            if (isInited) return;
            foreach (var item in Crosshairs)
            {
                item.Init();
            }
            //Weapon.ControlledAnimator.SetTrigger(Weapon.TakeOut);
            Weapon.CCAnimator.SetAnimation(Weapon.TakeOut_HashCode);
            isInited = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ShowCoreWeaponAnimator(bool UseTakeOut=false)
        {

            //Weapon.ControlledAnimator.gameObject.SetActive(true);
            if (!Weapon.CCAnimator.gameObject.activeSelf)
            {
                Weapon.CCAnimator.gameObject.SetActive(true);
                if (isInited)
                {
                    //Weapon.ControlledAnimator.SetTrigger(Weapon.Idle);
                    if (!UseTakeOut)
                        Weapon.CCAnimator.SetAnimation(Weapon.Idle_HashCode);
                    else Weapon.CCAnimator.SetAnimation(Weapon.TakeOut_HashCode);
                }
            }

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HideCoreWeaponAnimator()
        {
            Weapon.CCAnimator.gameObject.SetActive(false);
            //Weapon.ControlledAnimator.gameObject.SetActive(false);
        }
        //bool a=true;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRecoil(float Recoil)
        {
            //a = true;
            this.Weapon.Recoil = Recoil;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            //if (a)
            {
                if(Weapon.WeaponMode== WeaponConstants.WEAPON_MODE_NORMAL)
                {
                    if (Weapon.Recoil > 0)
                    {
                        Weapon.Recoil -= Weapon.RecoilRecoverSpeed * DeltaTime;
                    }
                    else if (Weapon.Recoil != 0)
                    {
                        Weapon.Recoil = 0;
                    }
                }
                if (HeatBar != null)
                {
                    HeatBar.fillAmount = Weapon.Base.CurrentHeat / Weapon.Base.MaxHeat;
                }
                //a = false;
            }
            Weapon.OnFrame(DeltaTime, UnscaledDeltaTime);
            foreach (var item in Crosshairs)
            {
                item.UpdateCrosshair(Weapon.Recoil);
            }
        }
    }
}
