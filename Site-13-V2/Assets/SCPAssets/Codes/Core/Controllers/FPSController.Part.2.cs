using Site13Kernel.Core.CustomizedInput;
using System.Runtime.CompilerServices;

namespace Site13Kernel.Core.Controllers
{
    public partial class FPSController
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Movement(float DeltaTime, float UnscaledDeltaTime)
        {
            if (!isMoveLocked)
            {
                Run(DeltaTime);
                Move(DeltaTime);
            }
            if (!isRotateLocked)
                Rotation(DeltaTime);
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
