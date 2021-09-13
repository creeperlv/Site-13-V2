using CLUNL.Data.Serializables.CheckpointSystem;
using Site13Kernel.GameLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class BioEntity : ControlledBehavior, ICheckpointData
    {
        //public new BaseController Parent;

        public string Name;

        public string ProtoTypeID;

        public float MaxHP;
        public float CurrentHP;

        public float MaxShield;
        public float CurrentShield;

        public float ShieldRecoverSpeed;
        public float HPRecoverSpeed;

        public float ShieldRecoverDelay;
        public float HPRecoverDelay;

        public float ShieldRecoverCountDown;
        public float HPRecoverCountDown;

        public EntityController Controller;

        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Damage(float V)
        {
            var V2 = math.max(0,V-CurrentShield);
            CurrentShield=math.max(0,CurrentShield-V);
            CurrentHP = math.max(0, CurrentHP - V2);
            if (CurrentHP <= 0)
                Die();
            ShieldRecoverCountDown = ShieldRecoverDelay;
            HPRecoverCountDown = HPRecoverDelay;
        }
        /// <summary>
        /// If it returns true, it will breaks original Die function, means Destory and EntityController.Remove(this); will not be executed.
        /// </summary>
        public Func<bool> OnDie = null;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Die()
        {
            if (OnDie != null)
            {
                if (OnDie()) return;
            }
            Controller.DestroyEntity(this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetName()
        {
            return name;
        }

        public void Load(List<object> data)
        {

        }

        public List<object> Save()
        {
            return new List<object> { ProtoTypeID,
                MaxHP,
                CurrentHP,
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
