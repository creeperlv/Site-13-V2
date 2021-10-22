using Site13Kernel.Core;
using Site13Kernel.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class ControlledGrenade:ControlledBehavior
    {
        public BaseGrenade baseGrenade;
        public Rigidbody Rigidbody;
        public GrenadeController ParentController;
        float TimeD;
        bool Triggered;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            TimeD += DeltaTime;
            if(TimeD>= baseGrenade.DetonationDuration)
            {
                if (!Triggered)
                {
                    Triggered = true;
                }
            }
            if (TimeD >= baseGrenade.ExistenceDuration)
            {
                ParentController.DestoryGrenade(this);
            }
        }

    }
    /// <summary>
    /// Suggested to be ran in ChunkDifferentialFrameSyncController.
    /// </summary>
    public class GrenadeController : ControlledBehavior
    {
        public List<ControlledGrenade> ControlledGrenades=new List<ControlledGrenade>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Instantiate(GameObject gameObject,Vector3 Position,Quaternion Rotation,Vector3 ForceDirection,ForceMode forceMode)
        {

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            foreach (var item in ControlledGrenades)
            {
                item.Refresh(DeltaTime, UnscaledDeltaTime);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestoryGrenade(ControlledGrenade CG)
        {
            ControlledGrenades.Remove(CG);
            Destroy(CG.gameObject);
        }
    }
}
