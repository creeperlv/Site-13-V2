using Site13Kernel.Core;
using Site13Kernel.Core.Interactives;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class Pickupable : InteractiveBase
    {
        public PickupItem ItemType;
        public Weapon Weapon;
        public int GrenadeID;
        public override void Operate(float DeltaTime, float UnscaledDeltaTime, DamagableEntity Operator)
        {
            {
                var Holder = Operator.GetComponent<BagHolder>();
                if (Holder != null)
                {
                    ControlledWeapon GeneratedWeapon;
                    Debugger.CurrentDebugger.Log($"Giving {Weapon.WeaponID} to {Holder.name}");
                    if (Holder.Weapon0 == null)
                    {
                        GeneratedWeapon = Holder.Weapon0 = GameObject.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Weapon.WeaponID].FPSPrefab, Holder.WeaponTransform).GetComponent<ControlledWeapon>();
                        Holder.CurrentWeapon = 0;
                        if (Holder.OnSwapWeapon != null)
                            Holder.OnSwapWeapon();
                    }
                    else
                    if (Holder.Weapon1 == null)
                    {

                        GeneratedWeapon = Holder.Weapon1 = GameObject.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Weapon.WeaponID].FPSPrefab, Holder.WeaponTransform).GetComponent<ControlledWeapon>();
                        Holder.CurrentWeapon = 1;
                        if (Holder.OnSwapWeapon != null)
                            Holder.OnSwapWeapon();
                    }
                    else
                    {
                        if (Holder.CurrentWeapon == 0)
                        {
                            {

                                var G = GameObject.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Holder.Weapon0.Weapon.Base.WeaponID].PickablePrefab, Holder.WeaponTransform).GetComponent<ControlledWeapon>();
                                G.Weapon.Base = Holder.Weapon0.Weapon.Base;
                            }
                            GameObject.Destroy(Holder.Weapon0.gameObject);

                            GeneratedWeapon = Holder.Weapon0 = GameObject.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Weapon.WeaponID].FPSPrefab, Holder.WeaponTransform).GetComponent<ControlledWeapon>();
                            if (Holder.OnSwapWeapon != null)
                                Holder.OnSwapWeapon();
                        }
                        else
                        {
                            {

                                var G = GameObject.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Holder.Weapon1.Weapon.Base.WeaponID].PickablePrefab, Holder.WeaponTransform).GetComponent<ControlledWeapon>();
                                G.Weapon.Base = Holder.Weapon1.Weapon.Base;
                            }

                            GameObject.Destroy(Holder.Weapon1.gameObject);

                            GeneratedWeapon = Holder.Weapon1 = GameObject.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Weapon.WeaponID].FPSPrefab, Holder.WeaponTransform).GetComponent<ControlledWeapon>();
                            if (Holder.OnSwapWeapon != null)
                                Holder.OnSwapWeapon();
                        }
                    }
                    GeneratedWeapon.Weapon.Base = Weapon;

                    if (this.Parent != null)
                    {
                        this.Parent.UnregisterFixedRefresh(this);
                        this.Parent.UnregisterRefresh(this);

                    }
                    Destroy(this.gameObject);
                }
                else
                {
                    Debugger.CurrentDebugger.LogError($">>Who whish to pick me up?\r\n<<{Operator.name}");
                }

            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ObtainRemaining(BagHolder holder)
        {
            if (ItemType == PickupItem.Weapon)
            {
                if (holder.Weapon0 != null)
                {
                    if (holder.Weapon0.Weapon.Base.WeaponID == Weapon.WeaponID)
                    {
                        __ObtainRemaining(holder.Weapon0.Weapon.Base);

                    }
                }
                if (holder.Weapon1 != null)
                {
                    if (holder.Weapon1.Weapon.Base.WeaponID == Weapon.WeaponID)
                    {
                        __ObtainRemaining(holder.Weapon1.Weapon.Base);
                    }
                }
            }
            else if (ItemType == PickupItem.Grenade)
            {
                Debugger.CurrentDebugger.Log("Giving Grenade...");
                bool isMatched = false;
                if (holder.Grenade0.GrenadeHashCode != -1)
                {
                    Debugger.CurrentDebugger.Log("Giving Grenade...Position 0");
                    isMatched = __ObtainRemaining(holder.Grenade0);
                }
                else if (holder.Grenade1.GrenadeHashCode != -1)
                {
                    Debugger.CurrentDebugger.Log("Giving Grenade...Position 1");
                    isMatched = __ObtainRemaining(holder.Grenade1);
                }
                if (isMatched == false)
                {
                    if (holder.Grenade0.GrenadeHashCode == -1)
                    {
                        holder.Grenade0 = new ProcessedGrenade
                        {
                            GrenadeHashCode = GrenadeID,
                            MaxCount = GrenadePool.CurrentPool.GrenadeItemMap[GrenadeID].Reference.MaxCount,
                            RemainingCount = 1
                        };
                        Destroy(this.gameObject);
                    }
                    else
                    if (holder.Grenade1.GrenadeHashCode == -1)
                    {
                        holder.Grenade1 = new ProcessedGrenade
                        {
                            GrenadeHashCode = GrenadeID,
                            MaxCount = GrenadePool.CurrentPool.GrenadeItemMap[GrenadeID].Reference.MaxCount,
                            RemainingCount = 1
                        };
                        Destroy(this.gameObject);
                    }
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool __ObtainRemaining(ProcessedGrenade PG)
        {

            if (PG.GrenadeHashCode == this.GrenadeID)
            {
                if (PG.MaxCount > PG.RemainingCount)
                {
                    PG.RemainingCount++;
                    Destroy(this.gameObject);
                    return true;
                }
                return true;
            }
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void __ObtainRemaining(Weapon PW)
        {
            var DELTA = PW.MaxCapacity - PW.CurrentBackup;
            if (DELTA > 0)
            {
                var ADDUP0 = Mathf.Min(DELTA, Weapon.CurrentBackup);

                Weapon.CurrentBackup -= ADDUP0;

                var ADDUP1 = Mathf.Min(DELTA - ADDUP0, Weapon.CurrentMagazine);

                Weapon.CurrentMagazine -= ADDUP1;
                PW.CurrentBackup += ADDUP0 + ADDUP1;

                if (Weapon.CurrentMagazine == 0 && Weapon.CurrentBackup == 0)
                {

                    if (this.Parent != null)
                    {
                        this.Parent.UnregisterFixedRefresh(this);
                        this.Parent.UnregisterRefresh(this);

                    }
                    Destroy(this.gameObject);
                }
            }
        }
    }
    public enum PickupItem
    {
        Weapon, Grenade
    }
}
