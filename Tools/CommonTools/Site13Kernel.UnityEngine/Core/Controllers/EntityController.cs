using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class EntityController : ControlledBehavior
    {
        public List<DamagableEntity> ControlledEntities;

        public override void Init()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Register(DamagableEntity entity)
        {
            entity.Controller = this;
            ControlledEntities.Add(entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            for (int i = ControlledEntities.Count - 1; i > 0; i--)
            {
                ControlledEntities[i].Refresh(DeltaTime, UnscaledDeltaTime);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestoryAll()
        {

            for (int i = ControlledEntities.Count - 1; i > 0; i--)
            {
                var entity = ControlledEntities[i];
                ControlledEntities.Remove(entity);
                Destroy(entity.gameObject);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestroyEntity(DamagableEntity entity)
        {
            ControlledEntities.Remove(entity);
            Destroy(entity.gameObject);
        }
    }
}
