using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using Site13Kernel.GameLogic;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.Utilities;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Debug = Site13Kernel.Diagnostics.Debug;

namespace Site13Kernel.Core.Controllers
{
    public partial class FPSController
    {
        public Action OnDeath = null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CancelRun()
        {
            MovingState = MovingState == MoveState.Crouch ? MovingState : MoveState.Walk;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FlashLight()
        {
            if (InputProcessor.GetInputDown("FlashLight"))
            {
                if (FlashLightObject != null)
                {
                    FlashLightObject.SetActive(!FlashLightObject.activeSelf);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WeaponControl(float DeltaTime, float UnscaledDeltaTime)
        {
            if (MovingState == MoveState.Crouch || MovingState == MoveState.Walk)
                if (InputProcessor.GetInputDown("SwitchWeapon"))
                {
                    TrySwitchWeapon();
                }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TrySwitchWeapon()
        {
            if (Weapon.Weapon.WeaponMode == WeaponConstants.WEAPON_MODE_RELOAD_STAGE_0 || Weapon.Weapon.WeaponMode == WeaponConstants.WEAPON_MODE_RELOAD_STAGE_1)
            {
                return;
            }
            BagHolder.CurrentWeapon = (BagHolder.CurrentWeapon == 1 ? 0 : 1);
            BagHolder.VerifyWeaponSlot();
            ApplySwitchWeapon();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplySwitchWeapon()
        {
            if (BagHolder.CurrentWeapon == 0)
            {
                W_HUD0.isPrimary = true;
                W_HUD1.isPrimary = false;
                if (BagHolder.Weapon0 != null)
                {
                    if (!BagHolder.Weapon0.gameObject.activeSelf)
                    {
                        Weapon =
                            BagHolder.Weapon0;
                        BagHolder.Weapon0.Unfire();
                        BagHolder.Weapon0.Weapon.ResetTakeOut();
                        BagHolder.Weapon0.gameObject.SetActive(true);
                    }
                    if (CurrentEntity.Shields.Count > 1)
                        CurrentEntity.Shields[1] = BagHolder.Weapon0.FPSRenderer;
                }
                if (BagHolder.Weapon1 != null)
                {
                    if (BagHolder.Weapon1.gameObject.activeSelf)
                    {
                        BagHolder.Weapon1.Unfire();
                        BagHolder.Weapon1.Weapon.ResetTakeOut();
                        BagHolder.Weapon1.gameObject.SetActive(false);
                    }
                }

            }
            else if (BagHolder.CurrentWeapon == 1)
            {

                W_HUD0.isPrimary = false;
                W_HUD1.isPrimary = true;

                if (BagHolder.Weapon1 != null)
                {
                    if (!BagHolder.Weapon1.gameObject.activeSelf)
                    {
                        Weapon =
                            BagHolder.Weapon1;
                        BagHolder.Weapon1.Unfire();
                        BagHolder.Weapon1.Weapon.ResetTakeOut();
                        BagHolder.Weapon1.gameObject.SetActive(true);
                    }
                    if (CurrentEntity.Shields.Count > 1)
                        CurrentEntity.Shields[1] = BagHolder.Weapon1.FPSRenderer;
                }
                if (BagHolder.Weapon0 != null)
                {
                    if (BagHolder.Weapon0.gameObject.activeSelf)
                    {
                        BagHolder.Weapon0.Unfire();
                        BagHolder.Weapon0.Weapon.ResetTakeOut();
                        BagHolder.Weapon0.gameObject.SetActive(false);
                    }
                }
            }
            if (Weapon != null)
            {
                _WalkPosition = Weapon.NormalPosition;
                _RunPosition = Weapon.RunningPosition;
            }

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Movement(float DeltaTime, float UnscaledDeltaTime)
        {
            if (!isMoveLocked)
            {
                Crouch(DeltaTime);
                Run(DeltaTime);
                Move(DeltaTime);
            }
            if (!isRotateLocked)
                Rotation(DeltaTime);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Crouch(float DeltaTime)
        {
            if (InputProcessor.GetInputDown("Crouch"))
            {
                if (MovingState == MoveState.Walk)
                {
                    SetState(MoveState.Crouch);
                }
            }
            if (InputProcessor.GetInputUp("Crouch"))
            {
                if (MovingState == MoveState.Crouch)
                {
                    SetState(MoveState.Walk);
                }
                //isRunning = false;
            }
            if (cc.isGrounded)
            {
                if (MovingState == MoveState.Crouch)
                {
                    MoveTransform(RealMainCam, CrouchHeadPosition, DeltaTime);
                }
                else
                {
                    MoveTransform(RealMainCam, NormalHeadPosition, DeltaTime);
                }

            }
            if (MovingState == MoveState.Crouch)
            {
                if (cc.height > CrouchHeight)
                {
                    cc.height -= DeltaTime * HeightExchangeSpeed;

                }
                else
                {
                    cc.height = CrouchHeight;
                }

                if (CurrentCollider.height > CrouchHeight)
                {
                    CurrentCollider.height -= DeltaTime * HeightExchangeSpeed;

                }
                else
                {
                    CurrentCollider.height = CrouchHeight;
                }
                //MoveTransform(RealMainCam, CrouchHeadPosition, DeltaTime);
            }
            else
            {
                if (cc.height < NormalHeight)
                {
                    cc.height += DeltaTime * HeightExchangeSpeed;
                }
                else
                {
                    cc.height = NormalHeight;
                }
                if (CurrentCollider.height < NormalHeight)
                {
                    CurrentCollider.height += DeltaTime * HeightExchangeSpeed;
                }
                else
                {
                    CurrentCollider.height = NormalHeight;
                }
                //MoveTransform(RealMainCam, NormalHeadPosition, DeltaTime);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MoveTransform(Transform OperatingTransform, Vector3 Target, float DeltaTime)
        {
            if (OperatingTransform.localPosition != Target)
            {
                if ((OperatingTransform.localPosition - Target).magnitude < .1f * HeadExchangeSpeed)
                {
                    OperatingTransform.localPosition = Target;
                }
                else
                {
                    OperatingTransform.localPosition += (Target - OperatingTransform.localPosition) * DeltaTime * HeadExchangeSpeed;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateHUD(float DT, float UDT)
        {
            if (CurrentEntity != null)
            {

                if (HP != null)
                {
                    HP.Value = CurrentEntity.CurrentHP;
                    HP.MaxValue = CurrentEntity.MaxHP;
                }
                if (Shield.Count > 0)
                {
                    foreach (var item in Shield)
                    {
                        item.Value = CurrentEntity.CurrentShield;
                        item.MaxValue = CurrentEntity.MaxShield;
                    }
                }
            }
            W_HUD0.Refresh(DT, UDT);
            W_HUD1.Refresh(DT, UDT);
            G_HUD0.Refresh(DT, UDT);
            G_HUD1.Refresh(DT, UDT);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GiveWeapon(Weapon TargetWeapon)
        {
            Weapon Weapon = TargetWeapon.Duplicate();
            var Holder = BagHolder;
            if (Holder != null)
            {
                ControlledWeapon GeneratedWeapon;
                if (Holder.Weapon0 == null)
                {
                    if (Holder.Weapon1 != null)
                    {
                        if (Weapon.WeaponID == Holder.Weapon1.Weapon.Base.WeaponID)
                        {
                            return;
                        }
                    }
                    GeneratedWeapon = Holder.Weapon0 = ObjectGenerator.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Weapon.WeaponID].FPSPrefab, Holder.WeaponTransform).GetComponent<ControlledWeapon>();

                    Holder.CurrentWeapon = 0;
                    try
                    {

                        if (Holder.OnSwapWeapon != null)
                            Holder.OnSwapWeapon();
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
                else
                if (Holder.Weapon1 == null)
                {
                    if (Weapon.WeaponID == Holder.Weapon0.Weapon.Base.WeaponID)
                    {
                        return;
                    }
                    GeneratedWeapon = Holder.Weapon1 = ObjectGenerator.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Weapon.WeaponID].FPSPrefab, Holder.WeaponTransform).GetComponent<ControlledWeapon>();

                    Holder.CurrentWeapon = 1;
                    if (Holder.OnSwapWeapon != null)
                        Holder.OnSwapWeapon();
                }
                else
                {
                    if (Holder.CurrentWeapon == 0)
                    {
                        {

                            if (Weapon.WeaponID == Holder.Weapon0.Weapon.Base.WeaponID)
                            {
                                return;
                            }
                            if (Weapon.WeaponID == Holder.Weapon1.Weapon.Base.WeaponID)
                            {
                                return;
                            }
                            var t = Holder.Weapon0.Weapon.Base.CurrentBackup +
                                                        Holder.Weapon0.Weapon.Base.CurrentMagazine;
                            if (t > 0)
                            {
                                var G = ObjectGenerator.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Holder.Weapon0.Weapon.Base.WeaponID].PickablePrefab, WeaponPool.CurrentPool.transform);
                                var P = G.GetComponentInChildren<Pickupable>();
                                G.transform.position = Holder.transform.position;
                                P.Weapon = Holder.Weapon0.Weapon.Base;
                            }
                        }
                        GameObject.Destroy(Holder.Weapon0.gameObject);

                        GeneratedWeapon = Holder.Weapon0 = ObjectGenerator.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Weapon.WeaponID].FPSPrefab, Holder.WeaponTransform).GetComponent<ControlledWeapon>();
                        GeneratedWeapon.transform.localPosition = GeneratedWeapon.NormalPosition;
                        if (Holder.OnSwapWeapon != null)
                            Holder.OnSwapWeapon();
                    }
                    else
                    {
                        {

                            if (Weapon.WeaponID == Holder.Weapon1.Weapon.Base.WeaponID)
                            {
                                return;
                            }
                            if (Weapon.WeaponID == Holder.Weapon1.Weapon.Base.WeaponID)
                            {
                                return;
                            }
                            var t = Holder.Weapon1.Weapon.Base.CurrentBackup +
                                                        Holder.Weapon1.Weapon.Base.CurrentMagazine;
                            if (t > 0)
                            {
                                var G = ObjectGenerator.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Holder.Weapon1.Weapon.Base.WeaponID].PickablePrefab, WeaponPool.CurrentPool.transform);
                                var P = G.GetComponentInChildren<Pickupable>();
                                G.transform.position = Holder.transform.position;
                                P.Weapon = Holder.Weapon1.Weapon.Base;
                            }
                        }
                        Destroy(Holder.Weapon1.gameObject);
                        GeneratedWeapon = Holder.Weapon1 = ObjectGenerator.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Weapon.WeaponID].FPSPrefab, Holder.WeaponTransform).GetComponentInChildren<ControlledWeapon>();
                        GeneratedWeapon.transform.localPosition = GeneratedWeapon.NormalPosition;
                        if (Holder.OnSwapWeapon != null)
                            Holder.OnSwapWeapon();
                    }
                }
                GeneratedWeapon.Weapon.Base = Weapon;
                GeneratedWeapon.transform.localPosition = GeneratedWeapon.NormalPosition;
                GeneratedWeapon.transform.localEulerAngles = GeneratedWeapon.NormalRotationEuler;
            }
            else
            {
            }

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            Interrupt00 = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            Interrupt00 = false;
        }
    }
    [Serializable]
    public class HoldGrenade
    {
        public int HashCode;
        public GameObject GrenadeMode;
    }
}
