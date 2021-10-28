using Site13Kernel.Data;
using Site13Kernel.GameLogic.Effects;
using Site13Kernel.Utilities;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class EffectController : ControlledBehavior
    {
        public Dictionary<int, GameObject> EffectDefinitions = new Dictionary<int, GameObject>();
        public List<EffectDefinition> _EffectDefinitions = new List<EffectDefinition>();
        public List<BaseEffect> ControlledEffects = new List<BaseEffect>();
        public static EffectController CurrentEffectController = null;
        public static int MAX_SPAWNABLE_EFFECT_COUNT = int.MaxValue;
        public static void SetMaxSpawnableEffectCount(int V)
        {
            MAX_SPAWNABLE_EFFECT_COUNT = V;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Init()
        {
            foreach (var item in _EffectDefinitions)
            {
                EffectDefinitions.Add(item.HashCode, item.Effect);
            }
            CurrentEffectController = this;
            Parent.RegisterRefresh(this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GameObject Spawn(int HashCode, Vector3 Position, Quaternion Rotation, bool isRelatedScale = false)
        {
            return Spawn(EffectDefinitions[HashCode], Position, Rotation, Vector3.zero,isRelatedScale);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GameObject Spawn(GameObject Prefab, Vector3 Position, Quaternion Rotation, bool isRelatedScale = false)
        {
            return Spawn(Prefab, Position, Rotation, Vector3.zero, isRelatedScale);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GameObject Spawn(int HashCode, Vector3 Position, Quaternion Rotation, Vector3 Scale, bool isRelatedScale = false)
        {
            return Spawn(EffectDefinitions[HashCode], Position, Rotation, Scale, transform, isRelatedScale);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GameObject Spawn(GameObject Prefab, Vector3 Position, Quaternion Rotation, Vector3 Scale, bool isRelatedScale = false)
        {
            return Spawn(Prefab, Position, Rotation, Scale, transform, isRelatedScale);
        }
        int CurrentEffects = 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GameObject Spawn(int HashCode, Vector3 Position, Quaternion Rotation, Vector3 Scale, Transform Parent, bool isRelatedScale=false)
        {
            if (CurrentEffects >= MAX_SPAWNABLE_EFFECT_COUNT) return null;
            CurrentEffects++;
            var go = Instantiate(EffectDefinitions[HashCode], Position, Rotation, Parent);
            if (isRelatedScale)
            {
                go.transform.localScale = Scale.DVI(Parent.lossyScale);
            }
            else
            {
                go.transform.localScale = Scale;
            }
            var BE = go.GetComponent<BaseEffect>();
            BE.Init();
            ControlledEffects.Add(BE);
            return go;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GameObject Spawn(GameObject Prefab, Vector3 Position, Quaternion Rotation, Vector3 Scale, Transform Parent, bool isRelatedScale = false)
        {
            if (CurrentEffects >= MAX_SPAWNABLE_EFFECT_COUNT) return null;
            CurrentEffects++;
            var go = Instantiate(Prefab, Position, Rotation, Parent);
            if (isRelatedScale)
            {
                go.transform.localScale = Scale.DVI(Parent.lossyScale);
            }
            else
            {
                go.transform.localScale = Scale;
            }
            var BE = go.GetComponent<BaseEffect>();
            BE.Init();
            ControlledEffects.Add(BE);
            return go;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            for (int i = ControlledEffects.Count - 1; i >= 0; i--)
            {
                var item = ControlledEffects[i];
                if (item == null)
                {
                    ControlledEffects.RemoveAt(i);
                    continue;
                }
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
            CurrentEffects--;
            if (ControlledEffects.Contains(baseEffect))
                ControlledEffects.Remove(baseEffect);
            Destroy(baseEffect.gameObject);
        }
    }
}
