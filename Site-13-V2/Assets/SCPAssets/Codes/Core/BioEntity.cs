using Site13Kernel.Data;
using Site13Kernel.Data.IO;
using Site13Kernel.Data.Serializables;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class BioEntity : DamagableEntity, IContainsPureData
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
        public bool useShieldRecoverObject;
        public List<GameObject> ShieldRecoverObjects;

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
            if (useShieldRecoverObject)
            {
                if (ShieldRecoverCountDown <= 0)
                {
                    if (CurrentShield < MaxShield)
                    {
                        foreach (var item in ShieldRecoverObjects)
                        {
                            if (!item.activeSelf)
                            {
                                item.SetActive(true);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in ShieldRecoverObjects)
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
        public void HealthRegen(float V)
        {
            CurrentHP = math.min(CurrentHP + V, MaxHP);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ShieldRegen(float V)
        {
            CurrentShield = math.min(CurrentShield + V, MaxShield);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Damage(float V)
        {
            var V2 = math.max(0, V - CurrentShield);
            if (CurrentShield != 0)
            {
                CurrentShield = math.max(0, CurrentShield - V);
                if (CurrentShield == 0)
                    if (OnShieldDown != null)
                        OnShieldDown();
            }
            if (!isInvincible)
                CurrentHP = math.max(0, CurrentHP - V2);
            if (CurrentHP <= 0)
            {
                Die();
                return true;
            }
            ShieldRecoverCountDown = ShieldRecoverDelay;
            HPRecoverCountDown = HPRecoverDelay;
            if (OnTakingDamage != null)
                OnTakingDamage(V, V - V2, V2, CurrentShield, CurrentHP);
            return false;
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

        public new IPureData ObtainData()
        {
            SerializableBioEntity SBE = new SerializableBioEntity();
            SBE.CombatRelationGroup = CombatRelationGroup;
            SBE.CurrentShield = CurrentShield;
            SBE.MaxShield = MaxShield;
            SBE.ShieldRecoverCountDown = ShieldRecoverCountDown;
            SBE.ShieldRecoverDelay = ShieldRecoverDelay;
            SBE.ShieldRecoverSpeed = ShieldRecoverSpeed;
            SBE.HPRecoverCountDown = HPRecoverCountDown;
            SBE.HPRecoverDelay = HPRecoverDelay;
            SBE.HPRecoverSpeed = HPRecoverSpeed;
            return SBE;
        }

        public new void ApplyData(IPureData data)
        {
            if (data is SerializableBioEntity SBE)
            {
                CombatRelationGroup = SBE.CombatRelationGroup;
                CurrentShield = SBE.CurrentShield;
                MaxShield = SBE.MaxShield;
                ShieldRecoverCountDown = SBE.ShieldRecoverCountDown;
                ShieldRecoverDelay = SBE.ShieldRecoverDelay;
                ShieldRecoverSpeed = SBE.ShieldRecoverSpeed;
                HPRecoverCountDown = SBE.HPRecoverCountDown;
                HPRecoverDelay = SBE.HPRecoverDelay;
                HPRecoverSpeed = SBE.HPRecoverSpeed;
            }
        }
    }
}
