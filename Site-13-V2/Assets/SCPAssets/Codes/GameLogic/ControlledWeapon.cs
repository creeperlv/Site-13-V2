using Site13Kernel.Core;
using Site13Kernel.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic
{
    public class ControlledWeapon : ControlledBehavior
    {
        public Weapon Weapon;
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
        public float MaxRecoil = 2;
        public float RecoilRecoverSpeed = 5;
        public GameObject Projectile;
        bool PseudoFire0 = false;
        bool PseudoFire1 = false;
        public List<ControlledCrosshair> Crosshairs;
        public float Recoil = 0;
        public GameObject HUDCanvas;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fire()
        {
            PseudoFire0 = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unfire()
        {
            PseudoFire0 = false;
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
            this.Recoil = Recoil;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            //if (a)
            {

                if (Recoil > 0)
                {
                    Recoil -= RecoilRecoverSpeed * DeltaTime;
                }
                else if (Recoil != 0)
                {
                    Recoil = 0;
                }
                //a = false;
            }
            foreach (var item in Crosshairs)
            {
                item.UpdateCrosshair(Recoil);
            }
        }
    }
}
