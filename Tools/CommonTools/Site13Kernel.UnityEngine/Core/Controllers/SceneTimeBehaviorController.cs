using Site13Kernel.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class SceneTimeBehaviorController : BaseController
    {
        public static SceneTimeBehaviorController CurrentSceneTimeBehaviorController;

        void Start()
        {
            SerializeAll();
            foreach (var item in _OnInit)
            {
                try
                {
                    InitBehavior(item);
                }
                catch (System.Exception e)
                {
                    Debugger.CurrentDebugger.Log(e, LogLevel.Error);
                }
            }
            foreach (var item in _OnInit)
            {
                if (INTERRUPT_INIT) continue;
                try
                {
                    item.Init();

                }
                catch (System.Exception e)
                {
                    Debugger.CurrentDebugger.Log(e, LogLevel.Error);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Instantiate(GameObject gameObject)
        {
            var go = GameObject.Instantiate(gameObject);
            var Behaviors = go.GetComponents<ControlledBehavior>();
            foreach (var item in Behaviors)
            {
                InitBehavior(item);
            }
            foreach (var item in Behaviors)
            {
                item.Init();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation)
        {
            var go = GameObject.Instantiate(gameObject, Position, Rotation);
            var Behaviors = go.GetComponents<ControlledBehavior>();
            foreach (var item in Behaviors)
            {
                InitBehavior(item);
            }
            foreach (var item in Behaviors)
            {
                item.Init();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Instantiate(GameObject gameObject, Vector3 Position, Quaternion Rotation, Transform parent)
        {
            var go = GameObject.Instantiate(gameObject, Position, Rotation, parent);
            var Behaviors = go.GetComponents<ControlledBehavior>();
            foreach (var item in Behaviors)
            {
                InitBehavior(item);
            }
            foreach (var item in Behaviors)
            {
                item.Init();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Instantiate(GameObject gameObject, Transform parent)
        {
            var go = GameObject.Instantiate(gameObject, parent);
            var Behaviors = go.GetComponents<ControlledBehavior>();
            foreach (var item in Behaviors)
            {
                InitBehavior(item);
            }
            foreach (var item in Behaviors)
            {
                item.Init();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Instantiate(GameObject gameObject, Transform parent, bool isWorld)
        {
            var go = GameObject.Instantiate(gameObject, parent, isWorld);
            var Behaviors = go.GetComponents<ControlledBehavior>();
            foreach (var item in Behaviors)
            {
                InitBehavior(item);
            }
            foreach (var item in Behaviors)
            {
                item.Init();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destory(GameObject gameObject)
        {
            var Behaviors = gameObject.GetComponents<ControlledBehavior>();
            foreach (var item in Behaviors)
            {
                UnregisterFixedRefresh(item);
                UnregisterRefresh(item);
            }
        }
        void OnDestory()
        {
            CurrentSceneTimeBehaviorController = null;
        }
        void Update()
        {
            float DeltaTime = Time.deltaTime;
            float UDeltaTime = Time.unscaledDeltaTime;
            foreach (var item in _OnRefresh)
            {
                if (INTERRUPT_REFRESH) continue;
#if DEBUG
                try
                {
#endif
                item.Refresh(DeltaTime, UDeltaTime);
#if DEBUG
                }
                catch (System.Exception e)
                {
                    Debugger.CurrentDebugger.Log(e, LogLevel.Error);
                }
#endif
            }
        }
        private void FixedUpdate()
        {

            float DeltaTime = Time.fixedDeltaTime;
            float UDeltaTime = Time.fixedUnscaledDeltaTime;
            foreach (var item in _OnFixedRefresh)
            {
                if (INTERRUPT_FIXED_REFRESH) continue;
#if DEBUG
                try
                {
#endif
                item.FixedRefresh(DeltaTime, UDeltaTime);
#if DEBUG
                }
                catch (System.Exception e)
                {
                    Debugger.CurrentDebugger.Log(e, LogLevel.Error);
                }
#endif
            }
        }
    }
}
