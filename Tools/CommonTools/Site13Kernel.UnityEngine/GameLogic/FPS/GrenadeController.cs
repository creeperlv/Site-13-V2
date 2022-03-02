using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    /// <summary>
    /// Suggested to be ran in ChunkDifferentialFrameSyncController.
    /// </summary>
    public class GrenadeController : ControlledBehavior
    {
        public List<ControlledGrenade> ControlledGrenades = new List<ControlledGrenade>();
        public static GrenadeController CurrentController;
        public override void Init()
        {
            CurrentController = this;
            Parent.RegisterRefresh(this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Instantiate(PrefabReference gameObject, Vector3 Position, Quaternion Rotation, Vector3 ForceDirection, ForceMode forceMode)
        {
            var GO = ObjectGenerator.Instantiate(gameObject);
            GO.transform.position = Position;
            GO.transform.rotation = Rotation;
            GO.GetComponent<Rigidbody>().AddForce(ForceDirection, forceMode);
            var GRE = GO.GetComponent<ControlledGrenade>();
            GRE.ParentController = this;
            ControlledGrenades.Add(GRE);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            for (int i = ControlledGrenades.Count - 1; i >= 0; i--)
            {
                var item = ControlledGrenades[i];
                if (item != null)
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
