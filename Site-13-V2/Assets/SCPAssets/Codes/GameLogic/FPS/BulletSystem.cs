using Site13Kernel.Core;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class BulletSystem : ControlledBehavior
    {
        List<BaseBullet> ManagedBullets=new List<BaseBullet>();
        public override void Init()
        {
            Parent.RegisterRefresh(this);
            GameRuntime.CurrentGlobals.CurrentBulletSystem = this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddBullet(GameObject Perfab, Vector3 Position, Quaternion quaternion)
        {
            var B=Instantiate(Perfab, Position, quaternion, GameRuntime.BulletHolder).GetComponent<BaseBullet>();
            B.Parent = this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            for(int i=0; i<ManagedBullets.Count; i++)
            {
                var item=ManagedBullets[i];
                item.Refresh(DeltaTime, UnscaledDeltaTime);
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
            Destroy(B.gameObject);
            ManagedBullets.Remove(B);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestoryAll()
        {
            for(int i=ManagedBullets.Count-1; i>=0; i--)
            {
                DestoryBullet(i);
            }
        }
    }
}
