using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class BioEntity : DamagableEntity
    {
        //public new BaseController Parent;

        public int CombatRelationGroup;

        public float MaxShield;
        public float CurrentShield;

        public float ShieldRecoverSpeed;
        public float HPRecoverSpeed;

        public float ShieldRecoverDelay;
        public float HPRecoverDelay;

        public float ShieldRecoverCountDown;
        public float HPRecoverCountDown;

        public bool useShieldMat;
        public Color ShieldColor;
        public List<Renderer> Shields;
        public float Intensity;

        public bool useShieldDownObject;
        public List<GameObject> ShieldObjects;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            base.Refresh(DeltaTime, UnscaledDeltaTime);
            if (CurrentHP < MaxHP)
            {
                if (HPRecoverCountDown > 0)
                {
                    HPRecoverCountDown -= DeltaTime;
                }
                else
                {
                    CurrentHP += HPRecoverSpeed * DeltaTime;
                    CurrentHP = math.min(CurrentHP, MaxHP);
                }
            }

            if (CurrentShield < MaxShield)
            {
                if (ShieldRecoverCountDown > 0)
                {
                    ShieldRecoverCountDown -= DeltaTime;
                }
                else
                {
                    CurrentShield += ShieldRecoverSpeed * DeltaTime;
                    CurrentShield = math.min(CurrentShield, MaxShield);
                }
            }
            if (useShieldMat)
            {
                if (Shields.Count > 0)
                {
                    //var C = ShieldColor * (CurrentShield == 0 ? 0 : ((MaxShield - CurrentShield) / MaxShield)) * Intensity;
                    var F = (CurrentShield == 0 ? 0 : ((MaxShield - CurrentShield) / MaxShield)) * Intensity;
                    foreach (var Shield in Shields)
                    {
                        if (Shield == null) continue;
                        foreach (var item in Shield.materials)
                        {
                            item.SetFloat("_EmissionStrength", F);
                        }
                    }
                }
            }
            if (useShieldDownObject)
            {
                if (CurrentShield <= 0)
                {
                    foreach (var item in ShieldObjects)
                    {
                        if (!item.activeSelf)
                        {
                            item.SetActive(true);
                        }
                    }
                }
                else
                {
                    foreach (var item in ShieldObjects)
                    {
                        if (item.activeSelf)
                        {
                            item.SetActive(false);
                        }
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Damage(float V)
        {
            var V2 = math.max(0, V - CurrentShield);
            if (CurrentShield != 0)
            {
                CurrentShield = math.max(0, CurrentShield - V);
                if (CurrentShield == 0)
                    if (OnShieldDown != null)
                        OnShieldDown();
            }
            CurrentHP = math.max(0, CurrentHP - V2);
            if (CurrentHP <= 0)
            {
                Die();
                return;
            }
            ShieldRecoverCountDown = ShieldRecoverDelay;
            HPRecoverCountDown = HPRecoverDelay;
            if (OnTakingDamage != null)
                OnTakingDamage(V, V - V2, V2, CurrentShield, CurrentHP);
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
}
