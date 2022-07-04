using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data.IO;
using Site13Kernel.GameLogic.FPS;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class BaseBullet : ControlledBehavior, IData
    {
        public float BaseDamage = 50;
        public float WeakPointDamage = 50;
        public float LifeTime = 10;
        public GameObject Emitter;
        //public Collider c;
        [HideInInspector]
        public BulletSystem ParentSystem { get; set; }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {

            //c.attachedRigidbody.collisionDetectionMode= CollisionDetectionMode.
            LifeTime -= DeltaTime;
            Move(DeltaTime, UnscaledDeltaTime);
            if (LifeTime < 0)
            {
                ParentSystem.DestoryBullet(this);
            }
        }
        public virtual void Move(float DT, float UDT)
        {
        }
        private void OnCollisionEnter(Collision collision)
        {
        }
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.isTrigger) return;
            Hit(collision);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Hit(Collider collision)
        {
            var Hittable = collision.gameObject.GetComponent<IHittable>();


            if (Hittable != null)
            {
                if (((MonoBehaviour)Hittable).gameObject == Emitter)
                {
                    return;
                }
                EffectController.CurrentEffectController.Spawn(Hittable.HitEffectHashCode(), collision.ClosestPoint(transform.position), Quaternion.identity, Vector3.one);
            }
            else
            {
                EffectController.CurrentEffectController.Spawn(1, collision.ClosestPoint(transform.position), Quaternion.identity, Vector3.one);

            }
            Person EmitterPerson = null;
            Person HitedPerson = null;
            if (Emitter != null)
            {
                EmitterPerson = Emitter.GetComponentInChildren<Person>();
            }
            var Entity = collision.gameObject.GetComponent<DamagableEntity>();
            var WeakPoint = collision.gameObject.GetComponent<WeakPoint>();
            if (WeakPoint != null)
            {
                HitedPerson = WeakPoint.AttachedBioEntity.GetComponentInChildren<Person>();
                TrySpawnHitEffect();
                if (EmitterPerson != null)
                {
                    EmitterPerson.OnHitOther.Invoke();
                }
                if (HitedPerson != null)
                {
                    HitedPerson.OnHitByOther.Invoke();
                }
                if (WeakPoint.AttachedBioEntity.Damage(WeakPointDamage))
                {
                    if (EmitterPerson != null)
                    {
                        EmitterPerson.OnKillOther.Invoke();
                    }
                    if (HitedPerson != null)
                    {
                        HitedPerson.OnDie.Invoke();
                    }
                }
            }
            else if (Entity != null)
            {
                HitedPerson = collision.gameObject.GetComponentInChildren<Person>();
                TrySpawnHitEffect();
                if (EmitterPerson != null)
                {
                    EmitterPerson.OnHitOther.Invoke();
                }
                if (HitedPerson != null)
                {
                    HitedPerson.OnHitByOther.Invoke();
                }
                if (Entity.Damage(BaseDamage))
                {
                    if (EmitterPerson != null)
                    {
                        EmitterPerson.OnKillOther.Invoke();
                    }
                    if (HitedPerson != null)
                    {
                        HitedPerson.OnDie.Invoke();
                    }
                }
            }

            ParentSystem.DestoryBullet(this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TrySpawnHitEffect()
        {
            if (Emitter != null)
            {
                var FPS = Emitter.GetComponent<FPSController>();
                if (FPS != null)
                {
                    FPS.OnHit();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Save()
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Load(IData data)
        {
            if (data is BaseBullet __bullet)
            {
                LifeTime = __bullet.LifeTime;
                SideLoad(data);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SideGetObjectData(SerializationInfo info, StreamingContext context) { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SideLoad(IData data)
        {

        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LifeTime", LifeTime, typeof(float));
            SideGetObjectData(info, context);
        }
    }
}
