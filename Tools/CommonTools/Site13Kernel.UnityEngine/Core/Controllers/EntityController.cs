using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Utilities;
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
            for (int i = ControlledEntities.Count - 1; i >= 0; i--)
            {
                ControlledEntities[i].Refresh(DeltaTime, UnscaledDeltaTime);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestoryAll(bool DestoryUnregisteredChildren=false)
        {
            for (int i = ControlledEntities.Count - 1; i >= 0; i--)
            {
                var entity = ControlledEntities[i];
                ControlledEntities.Remove(entity);
                if (entity.ControlledObject == null)
                    Destroy(entity.gameObject);
                else Destroy(entity.ControlledObject);
            }
            if (DestoryUnregisteredChildren)
            {
                for (int i = transform.childCount-1; i >=0 ; i--)
                {
                    var _obj = transform.GetChild(i);
                    Destroy(_obj.gameObject);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, DamagableEntity) Instantiate(int HashCode)
        {
            var _Object = ObjectGenerator.Instantiate(HashCode);
            //var _Object = ObjectGenerator.Instantiate(EntityPrefabMap[HashCode], HashCode);
            var Entity = _Object.GetComponent<DamagableEntity>();
            Register(Entity);
            return (_Object, Entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, DamagableEntity) Instantiate(GameObject gameObject)
        {
            var _Object = ObjectGenerator.Instantiate(gameObject, -1);
            var Entity = _Object.GetComponent<DamagableEntity>();
            if (Entity != null)
                Register(Entity);
            return (_Object, Entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, DamagableEntity) Instantiate(int HashCode, Vector3 Position, Quaternion Rotation)
        {
            var _Object = ObjectGenerator.Instantiate(HashCode, Position, Rotation);
            var Entity = _Object.GetComponentInChildren<DamagableEntity>();
            if (Entity != null)
                Register(Entity);
            return (_Object, Entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, DamagableEntity) Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation);
            var Entity = _Object.GetComponentInChildren<DamagableEntity>();
            if (Entity != null)
                Register(Entity);
            return (_Object, Entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, DamagableEntity) Instantiate(int HashCode, Vector3 Position, Quaternion Rotation, Transform transform)
        {
            var _Object = ObjectGenerator.Instantiate(HashCode, Position, Rotation, transform);
            var Entity = _Object.GetComponentInChildren<DamagableEntity>();
            if (Entity != null)
                Register(Entity);
            return (_Object, Entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, DamagableEntity) Instantiate(PrefabReference Prefab, Vector3 Position, Quaternion Rotation, Transform transform)
        {
            var _Object = ObjectGenerator.Instantiate(Prefab, Position, Rotation, transform);
            var Entity = _Object.GetComponentInChildren<DamagableEntity>();
            if (Entity != null)
                Register(Entity);
            return (_Object, Entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, DamagableEntity) Instantiate(PrefabReference Prefab, Vector3 Position, Quaternion Rotation)
        {
            var _Object = ObjectGenerator.Instantiate(Prefab, Position, Rotation);
            var Entity = _Object.GetComponentInChildren<DamagableEntity>();
            if (Entity != null)
                Register(Entity);
            return (_Object, Entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (GameObject, DamagableEntity) Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, Transform transform)
        {
            var _Object = GameObject.Instantiate(gameObject, Position, Rotation, transform);
            var Entity = _Object.GetComponentInChildren<DamagableEntity>();
            if (Entity != null)
                Register(Entity);
            return (_Object, Entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestroyEntity(DamagableEntity entity)
        {
            ControlledEntities.Remove(entity);
            if (entity.ControlledObject == null)
                Destroy(entity.gameObject);
            else
            {
                Destroy(entity.ControlledObject);
            }
        }
    }
}
