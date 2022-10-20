using Site13Kernel.Core;
using Site13Kernel.Core.Interactives;
using Site13Kernel.Data;
using Site13Kernel.Data.IO;
using Site13Kernel.Diagnostics;
using Site13Kernel.GameLogic.Character;
using Site13Kernel.GameLogic.Customization;
using Site13Kernel.GameLogic.Level;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using static Site13Kernel.GameLogic.FPS.BasicWeapon;
using Debug = Site13Kernel.Diagnostics.Debug;

namespace Site13Kernel.GameLogic.FPS
{
    [Serializable]
    public class Pickupable : InteractiveBase, IData
    {
        public PickupItem ItemType;
        public Weapon Weapon;
        public int GrenadeID;
        public int EquipmentID;
        public GameObject ControlledEntity;
        public Action OnPickup = null;
        public bool AmmoSupplyOnly = false;
        public bool UseNewBipedSystem = false;
        public AmmoDisp AmmoDispType = AmmoDisp.None;
        public List<Renderer> AmmoRenderers;
        public List<Text> AmmoDispTexts;
        public GenericWeapon AssociatedGenericWeapon;
        public void NotifyWeaponAmmo()
        {
            switch (AmmoDispType)
            {
                case AmmoDisp.None:
                    break;
                case AmmoDisp.TwoDig:
                    {
                        AmmoRenderers[0].material.SetFloat("_DigitNum", Weapon.CurrentMagazine % 10);
                        AmmoRenderers[1].material.SetFloat("_DigitNum", Mathf.FloorToInt(Weapon.CurrentMagazine / 10));
                    }
                    break;
                case AmmoDisp.ThreeDig:
                    {

                        AmmoRenderers[0].material.SetFloat("_DigitNum", Weapon.CurrentMagazine % 10);
                        AmmoRenderers[1].material.SetFloat("_DigitNum", Mathf.FloorToInt(Weapon.CurrentMagazine / 10) % 10);
                        AmmoRenderers[2].material.SetFloat("_DigitNum", Mathf.FloorToInt(Weapon.CurrentMagazine / 100));
                    }
                    break;
                case AmmoDisp.Liner:
                    break;
                case AmmoDisp.Text:
                    break;
                default:
                    break;
            }
        }
        private void OnEnable()
        {
            NotifyWeaponAmmo();
        }
        void OperateV2(float DeltaTime, float UnscaledDeltaTime, DamagableEntity Operator)
        {
            if (Operator is BipedEntity biped)
            {
                biped.EntityBag.TryObatinWeapon(AssociatedGenericWeapon);
            }
            else
            {
                Debug.Log("This weapon can only be given to BipedEntity!");
            }
        }
        public override void Operate(float DeltaTime, float UnscaledDeltaTime, DamagableEntity Operator)
        {
            if (UseNewBipedSystem)
            {
                OperateV2(DeltaTime, UnscaledDeltaTime, Operator);
                return;
            }
            if (AmmoSupplyOnly) return;
            {
                var Holder = Operator.GetComponent<BagHolder>();
                if (Holder != null)
                {
                    ControlledWeapon GeneratedWeapon;
                    Debugger.CurrentDebugger.Log($"Giving {Weapon.WeaponID} to {Holder.name}");
                    if (Holder.Weapon0 == null)
                    {
                        if (Holder.Weapon1 != null)
                        {
                            if (Weapon.WeaponID == Holder.Weapon1.Weapon.Base.WeaponID)
                            {
                                return;
                            }
                        }
                        GeneratedWeapon = Holder.Weapon0 = ObjectGenerator.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Weapon.WeaponID].FPSPrefab, Holder.WeaponTransform).GetComponent<ControlledWeapon>();
                        Holder.CurrentWeapon = 0;
                        try
                        {

                            if (Holder.OnSwapWeapon != null)
                                Holder.OnSwapWeapon();
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }
                    else
                    if (Holder.Weapon1 == null)
                    {
                        if (Weapon.WeaponID == Holder.Weapon0.Weapon.Base.WeaponID)
                        {
                            return;
                        }
                        GeneratedWeapon = Holder.Weapon1 = ObjectGenerator.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Weapon.WeaponID].FPSPrefab, Holder.WeaponTransform).GetComponent<ControlledWeapon>();
                        Holder.CurrentWeapon = 1;
                        if (Holder.OnSwapWeapon != null)
                            Holder.OnSwapWeapon();
                    }
                    else
                    {
                        if (Holder.CurrentWeapon == 0)
                        {
                            {

                                if (Weapon.WeaponID == Holder.Weapon0.Weapon.Base.WeaponID)
                                {
                                    return;
                                }
                                if (Weapon.WeaponID == Holder.Weapon1.Weapon.Base.WeaponID)
                                {
                                    return;
                                }

                                var t = Holder.Weapon0.Weapon.Base.CurrentBackup + Holder.Weapon0.Weapon.Base.CurrentMagazine;
                                if (t > 0)
                                {

                                    var G = ObjectGenerator.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Holder.Weapon0.Weapon.Base.WeaponID].PickablePrefab, WeaponPool.CurrentPool.transform);
                                    var __CW = Holder.Weapon0.gameObject.GetComponentInChildren<CustomizableWeapon>();
                                    var P = G.GetComponentInChildren<Pickupable>();
                                    var CW = G.GetComponentInChildren<CustomizableWeapon>();
                                    if (CW != null && __CW != null)
                                    {
                                        CW.TargetWeaponCoating = __CW.TargetWeaponCoating;
                                        CW.ApplyCoating();
                                    }
                                    G.transform.position = Holder.transform.position;
                                    P.Weapon = Holder.Weapon0.Weapon.Base;

                                }
                            }
                            GameObject.Destroy(Holder.Weapon0.gameObject);

                            GeneratedWeapon = Holder.Weapon0 = ObjectGenerator.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Weapon.WeaponID].FPSPrefab, Holder.WeaponTransform).GetComponent<ControlledWeapon>();
                            if (Holder.OnSwapWeapon != null)
                                Holder.OnSwapWeapon();
                        }
                        else
                        {
                            {

                                if (Weapon.WeaponID == Holder.Weapon1.Weapon.Base.WeaponID)
                                {
                                    return;
                                }
                                if (Weapon.WeaponID == Holder.Weapon1.Weapon.Base.WeaponID)
                                {
                                    return;
                                }
                                var t = Holder.Weapon1.Weapon.Base.CurrentBackup + Holder.Weapon1.Weapon.Base.CurrentMagazine;
                                if (t > 0)
                                {
                                    var G = ObjectGenerator.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Holder.Weapon1.Weapon.Base.WeaponID].PickablePrefab, WeaponPool.CurrentPool.transform);
                                    var __CW = Holder.Weapon1.gameObject.GetComponentInChildren<CustomizableWeapon>();
                                    var P = G.GetComponentInChildren<Pickupable>();
                                    var CW = G.GetComponentInChildren<CustomizableWeapon>();
                                    if (CW != null && __CW != null)
                                    {
                                        CW.TargetWeaponCoating = __CW.TargetWeaponCoating;
                                        CW.ApplyCoating();
                                    }
                                    G.transform.position = Holder.transform.position;
                                    P.Weapon = Holder.Weapon1.Weapon.Base;
                                }
                            }

                            GameObject.Destroy(Holder.Weapon1.gameObject);

                            GeneratedWeapon = Holder.Weapon1 = ObjectGenerator.Instantiate(WeaponPool.CurrentPool.WeaponItemMap[Weapon.WeaponID].FPSPrefab, Holder.WeaponTransform).GetComponentInChildren<ControlledWeapon>();
                            if (Holder.OnSwapWeapon != null)
                                Holder.OnSwapWeapon();
                        }
                    }
                    GeneratedWeapon.Weapon.Base = Weapon;
                    GeneratedWeapon.transform.localPosition = GeneratedWeapon.NormalPosition;
                    GeneratedWeapon.transform.localEulerAngles = GeneratedWeapon.NormalRotationEuler;
                    if (this.Parent != null)
                    {
                        this.Parent.UnregisterFixedRefresh(this);
                        this.Parent.UnregisterRefresh(this);

                    }
                    Debugger.CurrentDebugger.Log($"Giving {Weapon.WeaponID} to {Holder.name}... Completed.");
                    if (OnPickup != null) OnPickup();
                    Destroy(ControlledEntity);
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
                if (holder.Grenades.TryGetValue(GrenadeID, out var pg))
                {
                    __ObtainRemaining(pg);
                }
                else
                {
                    holder.Grenades.Add(GrenadeID, new ProcessedGrenade()
                    {
                        GrenadeHashCode = GrenadeID,
                        MaxCount = GrenadePool.CurrentPool.GrenadeItemMap[GrenadeID].Reference.MaxCount,
                        RemainingCount = 1
                    });
                }
            }
            else if (ItemType == PickupItem.Equipment)
            {
                Debugger.CurrentDebugger.Log("Giving Equipment...");
                {
                    if (holder.Equipments.TryGetValue(EquipmentID, out var i))
                    {
                        int Max = int.MaxValue;
                        if (EquipmentManifest.Instance.EqupimentMap.TryGetValue(EquipmentID, out var def))
                        {
                            Max = def.MaxCount;
                        }
                        holder.Equipments[EquipmentID] = Mathf.Min(Max, i + 1);
                        if (holder.Equipments[EquipmentID] != i)
                        {
                            Destroy(ControlledEntity);
                        }
                    }
                    else
                    {
                        holder.Equipments.Add(EquipmentID, 1);
                        Destroy(ControlledEntity);
                    }
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool __ObtainRemaining(ProcessedGrenade PG)
        {

            PG.GrenadeHashCode = this.GrenadeID;
            {
                if (PG.MaxCount > PG.RemainingCount)
                {
                    PG.RemainingCount++;
                    if (OnPickup != null) OnPickup();
                    Destroy(ControlledEntity);
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
                    if (OnPickup != null) OnPickup();
                    Destroy(ControlledEntity);
                }
            }
        }

        public void Save()
        {
        }

        public void Load(IData data)
        {
        }
        internal void ____ObtainRemaining(GenericWeapon GW)
        {
            var PW = GW.WeaponData;
            var DELTA = PW.MaxCapacity - PW.CurrentBackup;
            if (DELTA > 0)
            {
                var Weapon = AssociatedGenericWeapon.WeaponData;   
                var ADDUP0 = Mathf.Min(DELTA, Weapon.CurrentBackup);
                Weapon.CurrentBackup -= ADDUP0;

                var ADDUP1 = Mathf.Min(DELTA - ADDUP0, Weapon.CurrentMagazine);

                Weapon.CurrentMagazine -= ADDUP1;
                PW.CurrentBackup += ADDUP0 + ADDUP1;

                if (Weapon.CurrentMagazine == 0 && Weapon.CurrentBackup == 0)
                {

                    if (OnPickup != null) OnPickup();
                    AssociatedGenericWeapon.SelfDestruction();
                }
            }
        }
        public void ObtainRemaining(BipedEntity Biped)
        {
            if (ItemType == PickupItem.Weapon)
            {
                foreach (var item in Biped.EntityBag.Weapons)
                {
                ____ObtainRemaining(item);

                }
            }
            else if (ItemType == PickupItem.Grenade)
            {
            }
            else
            {

            }
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ITEMTYPE", ItemType, typeof(PickupItem));
            info.AddValue("WEAPONDATA", Weapon, typeof(Weapon));
            info.AddValue("GRENADE", GrenadeID, typeof(int));
        }
    }
    public enum PickupItem
    {
        Weapon, Grenade, Equipment
    }
}
