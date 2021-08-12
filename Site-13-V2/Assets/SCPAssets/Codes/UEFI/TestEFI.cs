using Site13Kernel.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Site13Kernel.UEFI
{
    public class TestEFI : UEFIBase
    {
        public override async Task Run()
        {
            for (int i = 0; i < 50; i++)
            {
                await Task.Delay(100);
                Debugger.CurrentDebugger.Log($"Task Seq:{i}");
            }
        }
    }
}
