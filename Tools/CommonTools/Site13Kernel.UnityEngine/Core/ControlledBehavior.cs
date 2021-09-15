using Site13Kernel.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class ControlledBehavior : MonoBehaviour, IControllable
    {

        public virtual BaseController Parent
        {
            get; set;
        }
        public virtual void Init()
        {

        }
        public virtual void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {

        }
        public virtual void FixedRefresh(float DeltaTime, float UnscaledDeltaTime)
        {

        }
    }
    public interface IControllable
    {
        BaseController Parent
        {
            get;
            set;
        }
        void Init();
        void Refresh(float DeltaTime,float UnscaledDeltaTime);
        void FixedRefresh(float DeltaTime, float UnscaledDeltaTime);
    }
    public class SyncInitializer : MonoBehaviour
    {
        public bool isDone=false;
        public virtual void Init()
        {

        }
        public virtual void Execute()
        {

        }
        public virtual void FixedExecute()
        {

        }
    }
}
