using Site13Kernel.Core;
using Site13Kernel.Diagnostics;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class GlobalBioController : EntityController
    {
        public static GlobalBioController CurrentGlobalBioController;
        public Dictionary<string, GameObject> BioDefinitions = new Dictionary<string, GameObject>();
        public List<BioDefinition> Definitions = new List<BioDefinition>();
        public List<EntityItem> entityItems;
        public List<FoeDefinition> FoeRelationDefinitions = new List<FoeDefinition>();
        public Dictionary<int, List<int>> FoeRelations = new Dictionary<int, List<int>>();
        public override void Init()
        {
            base.Init();
            if (EntityPrefabMap == null) EntityPrefabMap = new Dictionary<int, GameObject>();
            foreach (var item in entityItems)
            {
                EntityPrefabMap.Add(item.HashCode, item.Prefab);
            }
            foreach (var item in Definitions)
            {
                BioDefinitions.Add(item.BioEntityID, item.ProtoType);
            }
            foreach (var item in FoeRelationDefinitions)
            {
                FoeRelations.Add(item.MainGroup, item.FoeGroups);
            }
            Parent.RegisterRefresh(this);
            CurrentGlobalBioController = this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isFoe(int TargetGroup, int RelatedToGroup)
        {
            if (FoeRelations.ContainsKey(RelatedToGroup))
            {
                return FoeRelations[RelatedToGroup].Contains(TargetGroup);
            }
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyFoeDefinitions(ref List<FoeDefinition> def)
        {
            FoeRelations.Clear();
            foreach (var item in def)
            {
                FoeRelations.Add(item.MainGroup, item.FoeGroups);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GameObject Spawn(string ID, Vector3 position, Vector3 Rotation)
        {
            var GO = ObjectGenerator.Instantiate(ID, position, Quaternion.Euler(Rotation), this.transform);
            var BIO = GO.GetComponentInChildren<BioEntity>();
            //BIO.Parent = this;
            if (BIO == null)
            {
                Debugger.CurrentDebugger.LogWarning("A non-standard entity is inited without DamagableEntity on it.");
                return GO;
            }
            BIO.Controller = this;
            BIO.Init();
            this.Register(BIO);
            //ControlledEntities.Add(BIO);

            return GO;
        }

    }
    [Serializable]
    public class FoeDefinition
    {
        public int MainGroup;
        public List<int> FoeGroups;
    }
    [Serializable]
    public class BioDefinition
    {
        public string BioEntityID;
        public GameObject ProtoType;
    }
}
