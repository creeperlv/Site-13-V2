using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class DefaultEntityController : EntityController
    {
        public List<EntityItem> entityItems;
        public override void Init()
        {
            base.Init();
            if (EntityPrefabMap == null) EntityPrefabMap = new Dictionary<int, GameObject>();
            foreach (var item in entityItems)
            {
                EntityPrefabMap.Add(item.HashCode, item.Prefab);
            }
            Parent.RegisterRefresh(this);
            GameRuntime.CurrentLocals.CurrentDefaultController = this;
        }
    }
    [Serializable]
    public class EntityItem
    {
        public int HashCode;
        public GameObject Prefab;
    }
}
