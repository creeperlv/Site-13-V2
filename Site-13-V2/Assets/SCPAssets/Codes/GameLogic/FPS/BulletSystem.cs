using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class BulletSystem : ControlledBehavior
    {
        List<BaseBullet> ManagedBullets = new List<BaseBullet>();
        public override void Init()
        {
            Parent.RegisterRefresh(this);
            GameRuntime.CurrentGlobals.CurrentBulletSystem = this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddBullet(PrefabReference Perfab, Vector3 Position, Quaternion rotation, GameObject Emitter = null)
        {
            var B = ObjectGenerator.Instantiate(Perfab, Position, rotation, GameRuntime.BulletHolder).GetComponent<BaseBullet>();
            B.ParentSystem = this;
            B.Emitter = Emitter;
            ManagedBullets.Add(B);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            for (int i = 0; i < ManagedBullets.Count; i++)
            {
                var item = ManagedBullets[i];
                if (item != null)
                {
                    item.Refresh(DeltaTime, UnscaledDeltaTime);

                }
                else ManagedBullets.RemoveAt(i);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestoryBullet(int I)
        {
            var item = ManagedBullets[I];
            Destroy(item.gameObject);
            ManagedBullets.Remove(item);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestoryBullet(BaseBullet B)
        {

            //Remove bullet reference before destorying it.
            ManagedBullets.Remove(B);

            Destroy(B.gameObject);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestoryAll()
        {
            for (int i = ManagedBullets.Count - 1; i >= 0; i--)
            {
                DestoryBullet(i);
            }
        }
    }
}
