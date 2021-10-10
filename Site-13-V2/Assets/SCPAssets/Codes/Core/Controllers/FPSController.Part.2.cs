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
            if (InputProcessor.CurrentInput.GetInputDown("Crouch") && toZoom == false)
            {
                if (MovingState == MoveState.Walk) MovingState = MoveState.Crouch;
            }
            if (InputProcessor.CurrentInput.GetInputUp("Crouch"))
            {
                if (MovingState == MoveState.Crouch) MovingState = MoveState.Walk;
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
