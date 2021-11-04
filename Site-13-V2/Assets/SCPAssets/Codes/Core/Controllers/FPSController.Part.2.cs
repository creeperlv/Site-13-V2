using Site13Kernel.Core.CustomizedInput;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public partial class FPSController
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CancelRun()
        {
            MovingState = MovingState == MoveState.Crouch ? MovingState : MoveState.Walk;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WeaponControl(float DeltaTime, float UnscaledDeltaTime)
        {
            if (MovingState == MoveState.Crouch || MovingState == MoveState.Walk)
                if (InputProcessor.CurrentInput.GetInputDown("SwitchWeapon"))
                {
                    TrySwitchWeapon();
                }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TrySwitchWeapon()
        {
            BagHolder.CurrentWeapon = (BagHolder.CurrentWeapon == 1 ? 0 : 1);
            BagHolder.VerifyWeaponSlot();
            ApplySwitchWeapon();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplySwitchWeapon()
        {
            if (BagHolder.CurrentWeapon == 0)
            {

                if (BagHolder.Weapon0 != null)
                {
                    if (!BagHolder.Weapon0.gameObject.activeSelf)
                    {
                        BagHolder.Weapon0.Unfire();
                        BagHolder.Weapon0.Weapon.ResetTakeOut();
                        BagHolder.Weapon0.gameObject.SetActive(true);
                    }
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

                if (BagHolder.Weapon1 != null)
                {
                    if (!BagHolder.Weapon1.gameObject.activeSelf)
                    {
                        BagHolder.Weapon1.Unfire();
                        BagHolder.Weapon1.Weapon.ResetTakeOut();
                        BagHolder.Weapon1.gameObject.SetActive(true);
                    }
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
            if (InputProcessor.CurrentInput.GetInputDown("Crouch"))
            {
                if (MovingState == MoveState.Walk)
                {
                    SetState(MoveState.Crouch);
                }
            }
            if (InputProcessor.CurrentInput.GetInputUp("Crouch"))
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
        public void UpdateHUD()
        {
            if (CurrentEntity != null)
            {

                if (HP != null)
                {
                    HP.Value = CurrentEntity.CurrentHP;
                    HP.MaxValue = CurrentEntity.MaxHP;
                }
                if (Shield != null)
                {
                    Shield.Value = CurrentEntity.CurrentShield;
                    Shield.MaxValue = CurrentEntity.MaxShield;
                }
            }
        }
    }
}
