using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core.TagSystem
{
    public class SystemInitializer : ControlledBehavior
    {
        public override void Init()
        {
            TagSystemManager.Instance.RegisterSystem(new TransformSyncSystem());
        }
    }
}
