using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic
{
    public class EntityController : ControlledBehavior
    {
        public List<BioEntity> ControlledEntities;

        public override void Init()
        {

        }

        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            for (int i = ControlledEntities.Count-1; i >0; i--)
            {
                ControlledEntities[i].Refresh(DeltaTime, UnscaledDeltaTime);
            }
        }
        public void DestroyEntity(BioEntity entity)
        {
            ControlledEntities.Remove(entity);
            Destroy(entity.gameObject);
        }
    }
}
