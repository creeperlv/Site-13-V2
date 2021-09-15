using Site13Kernel.GameLogic.Effects;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class EffectController : ControlledBehavior
    {
        public List<BaseEffect> ControlledEffects = new List<BaseEffect>();
        public override void Init()
        {

            GameRuntime.CurrentGlobals.CurrentEffectController = this;
            Parent.RegisterRefresh(this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Spawn(GameObject Prefab, Vector3 Position, Quaternion Rotation)
        {
            Spawn(Prefab, Position, Rotation,Vector3.zero);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Spawn(GameObject Prefab, Vector3 Position, Quaternion Rotation, Vector3 Scale)
        {
            Spawn(Prefab, Position, Rotation, Scale, transform);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Spawn(GameObject Prefab, Vector3 Position, Quaternion Rotation, Vector3 Scale,Transform Parent)
        {
            var go = Instantiate(Prefab, Position, Rotation, Parent);
            go.transform.localScale = Scale;
            ControlledEffects.Add(go.GetComponent<BaseEffect>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            for (int i = ControlledEffects.Count - 1; i >= 0; i--)
            {
                var item = ControlledEffects[i];
                item.TimeD += DeltaTime;
                item.Refresh(DeltaTime, UnscaledDeltaTime);
                if (item.TimeD >= item.LifeTime)
                {
                    DestroyEffect(item);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestroyEffect(BaseEffect baseEffect)
        {
            ControlledEffects.Remove(baseEffect);
            Destroy(baseEffect.gameObject);
        }
    }
}
