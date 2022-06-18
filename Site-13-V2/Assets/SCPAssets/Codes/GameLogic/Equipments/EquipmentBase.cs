using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Equipments
{
    public class EquipmentBase : ControlledBehavior
    {
        public BioEntity User;
        public GameObject AnimationPart;
        public float AnimationLength;
        public float ActionPoint;
        public float RegenAmount;
        public float ActionAmount;
        public EquipmentType equipmentType;
        public PrefabReference InitPrefabRef;
        public Transform InitPos;
        public bool AttachToPos;
        public virtual void Action()
        {
            switch (equipmentType)
            {
                case EquipmentType.InitPrefab:
                    {
                        if (AttachToPos)
                        {
                            ObjectGenerator.Instantiate(InitPrefabRef, InitPos.position, InitPos.rotation, InitPos);
                        }
                        else
                        {
                            ObjectGenerator.Instantiate(InitPrefabRef, InitPos.position, InitPos.rotation);
                        }
                    }
                    break;
                case EquipmentType.HealthRegen:
                    {
                        if(User != null)
                        {
                            User.HealthRegen(ActionAmount);
                        }
                    }
                    break;
                case EquipmentType.ShieldRegen:
                    {
                        if (User != null)
                        {
                            User.ShieldRegen(ActionAmount);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
    public enum EquipmentType
    {
        InitPrefab,
        HealthRegen,
        ShieldRegen,
    }
}
