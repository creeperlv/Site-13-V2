using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class ControlledBehaviorSet : ControlledBehavior
    {
        public bool UseRefresh;
        public bool UseFixedRefresh;
        public List<ControlledBehavior> SubBehaviors;
        BaseController _Parent;
        public override BaseController Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                _Parent = value;
            }
        }
        public override void Init()
        {
            if (UseFixedRefresh)
            {
                _Parent.RegisterFixedRefresh(this);
            }
            if (UseRefresh)
            {
                _Parent.RegisterRefresh(this);
            }
            foreach (var item in this.GetComponents<ControlledBehavior>())
            {
                if (item != this)
                {
                    SubBehaviors.Add(item);
                }
            }
            foreach (var item in SubBehaviors)
            {
                item.Parent = Parent;
            }
            foreach (var item in SubBehaviors)
            {
                item.Init();
            }
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            foreach (var item in SubBehaviors)
            {
                item.Refresh(DeltaTime,UnscaledDeltaTime);
            }
        }
        public override void FixedRefresh(float DeltaTime, float UnscaledDeltaTime)
        {
            foreach (var item in SubBehaviors)
            {
                item.FixedRefresh(DeltaTime,UnscaledDeltaTime);
            }
        }
    }
}
