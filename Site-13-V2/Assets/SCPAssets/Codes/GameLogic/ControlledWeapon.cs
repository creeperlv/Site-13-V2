using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.FPS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic
{
    public class ControlledWeapon : ControlledBehavior
    {
        public BasicWeapon Weapon;
        [Header("Zoom")]
        public bool CanZoom = false;
        public CanvasGroup ZoomHUD = null;
        public float ZoomFov = 50;
        [Header("Movement")]
        public Vector3 NormalPosition;
        public Vector3 RunningPosition;
        public Vector3 NormalRotationEuler;
        public Vector3 RunningRotationEuler;
        public Animator ControlledAnimator;
        public string TakeOut = "TakeOut";
        public string Combat = "Combat";
        public string Reload = "Reload";
        public string Idle = "Idle";
        public List<ControlledCrosshair> Crosshairs;
        public GameObject HUDCanvas;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fire()
        {
            Weapon.Fire();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unfire()
        {
            Weapon.Unfire();
        }
        bool isInited = false;
        public override void Init()
        {
            foreach (var item in Crosshairs)
            {
                item.Init();
            }
            ControlledAnimator.SetTrigger(TakeOut);
            isInited = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ShowCoreWeaponAnimator()
        {
            ControlledAnimator.gameObject.SetActive(true);
            if (isInited)
            {
                ControlledAnimator.SetTrigger(Idle);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HideCoreWeaponAnimator()
        {
            ControlledAnimator.gameObject.SetActive(false);
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

                if (Weapon.Recoil > 0)
                {
                    Weapon.Recoil -= Weapon.RecoilRecoverSpeed * DeltaTime;
                }
                else if (Weapon.Recoil != 0)
                {
                    Weapon.Recoil = 0;
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
