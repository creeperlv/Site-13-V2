using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class ControlledBehavior : MonoBehaviour
    {
        [HideInInspector]
        public BehaviorController Parent;
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

}
