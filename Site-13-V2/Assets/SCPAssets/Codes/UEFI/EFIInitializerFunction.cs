using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.UEFI
{
    public class EFIInitializerFunction : UEFIBase
    {
        public override void Init()
        {
            GameRuntime.CurrentGlobals.Init();
        }
    }
}
