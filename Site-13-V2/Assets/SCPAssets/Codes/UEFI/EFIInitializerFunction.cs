using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Site13Kernel.UEFI
{
    public class EFIInitializerFunction : UEFIBase
    {
        public GameObject BackgroundObject;
        public override void Init()
        {
            GameRuntime.CurrentGlobals.Init();
            GameObject.Instantiate(BackgroundObject, transform.root.parent, true);
        }
    }
}
