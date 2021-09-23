using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class EntityController : ControlledBehavior
    {
        public static Dictionary<int, GameObject> EntityPrefabMap;
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
        public (GameObject, DamagableEntity) Instantiate(int HashCode)
        {
            return Instantiate(EntityPrefabMap[HashCode]);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, DamagableEntity) Instantiate(GameObject gameObject)
        {
            var _Object = GameObject.Instantiate(gameObject);
            var Entity = _Object.GetComponent<DamagableEntity>();
            Register(Entity);
            return (_Object, Entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, DamagableEntity) Instantiate(int HashCode, Vector3 Position, Quaternion Rotation)
        {
            return Instantiate(EntityPrefabMap[HashCode], Position, Rotation);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, DamagableEntity) Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation);
            var Entity = _Object.GetComponent<DamagableEntity>();
            Register(Entity);
            return (_Object, Entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, DamagableEntity) Instantiate(int HashCode, Vector3 Position, Quaternion Rotation, Transform transform)
        {
            return Instantiate(EntityPrefabMap[HashCode], Position, Rotation, transform);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, DamagableEntity) Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, Transform transform)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation, transform);
            var Entity = _Object.GetComponent<DamagableEntity>();
            Register(Entity);
            return (_Object, Entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestroyEntity(DamagableEntity entity)
        {
            ControlledEntities.Remove(entity);
            Destroy(entity.gameObject);
        }
    }
}
