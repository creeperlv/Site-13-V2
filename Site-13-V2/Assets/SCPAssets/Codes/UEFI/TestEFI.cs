using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Site13Kernel.UEFI
{
    public class TestEFI : UEFIBase
    {
#if DEBUG
        public override void Init()
        {
            StartCoroutine(DelayedAction());
        }
        IEnumerator DelayedAction()
        {
            yield return null;
            //GameRuntime.CurrentGlobals.SubtitleController.ShowSubtitle(new Subtitle { ID = "", Fallback = "Test Subtitle", Duration = 4 });
        }
#endif
    }
}
