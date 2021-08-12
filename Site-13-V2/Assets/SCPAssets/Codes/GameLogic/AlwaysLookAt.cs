using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Site13Kernel.GameLogic
{
    public class AlwaysLookAt : ControlledBehavior
    {
        public Transform Target;
        public override void Refresh(float DeltaTime)
        {
            this.transform.LookAt(Target);
        }
    }

}