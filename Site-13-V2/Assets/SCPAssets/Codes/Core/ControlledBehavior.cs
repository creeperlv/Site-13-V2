using System;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class ControlledBehavior : MonoBehaviour, IControllable
    {

        public virtual BaseController Parent {
            get;set;
        }
        public virtual void Init()
        {

        }
        public virtual void Refresh(float DeltaTime)
        {

        }
        public virtual void FixedRefresh(float DeltaTime)
        {

        }
    }
    public interface IControllable
    {
        BaseController Parent {
            get;
            set;
        }
        void Init();
        void Refresh(float DeltaTime);
        void FixedRefresh(float DeltaTime);
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
