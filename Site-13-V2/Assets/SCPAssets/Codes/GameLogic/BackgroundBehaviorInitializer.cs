using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic
{
    public class BackgroundBehaviorInitializer : ControlledBehavior
    {
        public Transform BulletHolder;
        public override void Init()
        {
            GameRuntime.BulletHolder = BulletHolder;
        }
    }
}
