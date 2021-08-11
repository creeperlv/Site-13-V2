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

        void Start()
        {
            foreach (var item in OnInit)
            {
                InitBehavior(item);
            }
            foreach (var item in OnInit)
            {
                item.Init();
            }
        }
        public void InitBehavior(ControlledBehavior behavior)
        {
            behavior.Parent= this;
        }
        void Update()
        {
            float DeltaTime=Time.deltaTime;
            foreach (var item in OnRefresh)
            {
                item.Refresh(DeltaTime);
            }
        }
        private void FixedUpdate()
        {

            float DeltaTime=Time.fixedDeltaTime;
            foreach (var item in OnFixedRefresh)
            {
                item.FixedRefresh(DeltaTime);
            }
        }
    }
}
