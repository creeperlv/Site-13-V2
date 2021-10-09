using Site13Kernel.Core.CustomizedInput;
using System.Runtime.CompilerServices;

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
