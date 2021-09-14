using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Site13Kernel.Core
{
    public class BioEntity : DamagableEntity
    {
        //public new BaseController Parent;

        public float MaxShield;
        public float CurrentShield;

        public float ShieldRecoverSpeed;
        public float HPRecoverSpeed;

        public float ShieldRecoverDelay;
        public float HPRecoverDelay;

        public float ShieldRecoverCountDown;
        public float HPRecoverCountDown;

        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Damage(float V)
        {
            var V2 = math.max(0,V-CurrentShield);
            CurrentShield=math.max(0,CurrentShield-V);
            CurrentHP = math.max(0, CurrentHP - V2);
            if (CurrentHP <= 0)
                Die();
            ShieldRecoverCountDown = ShieldRecoverDelay;
            HPRecoverCountDown = HPRecoverDelay;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override List<object> SubSave()
        {
            return new List<object>{
                MaxShield,
                CurrentShield,
                ShieldRecoverSpeed,
                HPRecoverSpeed };
        }
    }
    public class WeakPoint : ControlledBehavior
    {
        public BioEntity AttachedBioEntity;
    }
}
