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
        public Transform ZoomEffectPoint;
        [Header("Movement")]
        public Vector3 NormalPosition;
        public Vector3 RunningPosition;
        public Vector3 NormalRotationEuler;
        public Vector3 RunningRotationEuler;
        public List<ControlledCrosshair> Crosshairs;
        public GameObject HUDCanvas;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Combat()
        {

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
        public void Reload()
        {
            Weapon.Reload();
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
            Weapon.ControlledAnimator.SetTrigger(Weapon.TakeOut);
            isInited = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ShowCoreWeaponAnimator()
        {

            Weapon.ControlledAnimator.gameObject.SetActive(true);
            if (isInited)
            {

                Weapon.ControlledAnimator.SetTrigger(Weapon.Idle);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HideCoreWeaponAnimator()
        {

            Weapon.ControlledAnimator.gameObject.SetActive(false);
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
