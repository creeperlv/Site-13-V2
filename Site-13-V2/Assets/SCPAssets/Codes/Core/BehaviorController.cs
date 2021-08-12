using Site13Kernel.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class BehaviorController : MonoBehaviour
    {
        public List<ControlledBehavior> OnInit=new List<ControlledBehavior>();
        public List<ControlledBehavior> OnRefresh=new List<ControlledBehavior>();
        public List<ControlledBehavior> OnFixedRefresh=new List<ControlledBehavior>();
        public bool CrossScene=false;
        void Start()
        {
            if (CrossScene)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            foreach (var item in OnInit)
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
            foreach (var item in OnInit)
            {
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
        public T GetBehavior<T>() where T: ControlledBehavior
        {
            foreach (var item in OnInit)
            {
                if (item is T t)
                {
                    return t;
                }
            }
            foreach (var item in OnRefresh)
            {
                if (item is T t)
                {
                    return t;
                }
            }
            foreach (var item in OnFixedRefresh)
            {
                if (item is T t)
                {
                    return t;
                }
            }
            return null;
        }
        public void InitBehavior(ControlledBehavior behavior)
        {
            behavior.Parent = this;
        }
        void Update()
        {
            float DeltaTime=Time.deltaTime;
            foreach (var item in OnRefresh)
            {
                    item.Refresh(DeltaTime);
                //try
                //{
                //    item.Refresh(DeltaTime);
                //}
                //catch (System.Exception e)
                //{
                //    Debugger.CurrentDebugger.Log(e, LogLevel.Error);
                //}
            }
        }
        private void FixedUpdate()
        {

            float DeltaTime=Time.fixedDeltaTime;
            foreach (var item in OnFixedRefresh)
            {
                    item.FixedRefresh(DeltaTime);
                //try
                //{
                //    item.FixedRefresh(DeltaTime);
                //}
                //catch (System.Exception e)
                //{
                //    Debugger.CurrentDebugger.Log(e, LogLevel.Error);
                //}
            }
        }
    }
}
