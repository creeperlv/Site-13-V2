using Site13Kernel.Core;
using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Site13Kernel.UEFI
{
    public class EFIInitializerFunction : UEFIBase
    {
        public GameObject BackgroundObject;
        public UniversalRenderPipelineAsset TargetAsset;
        public override void Init()
        {
            GameEnv.DataPath = Application.persistentDataPath;

            Settings.Init();

            GameRuntime.CurrentGlobals.Init();
            GameRuntime.CurrentGlobals.UsingAsset = TargetAsset;
            Instantiate(BackgroundObject);
        }
    }
}
