using Site13Kernel.Core;
using Site13Kernel.Data;
using System;
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
        public Configuration InitConfiguration;
        public override void Init()
        {
            GameEnv.DataPath = Application.persistentDataPath;
            GameEnv.CollisionDamageSpeedThreshold = InitConfiguration.CollisionDamageSpeedThreshold;
            GameEnv.CollisionDamageIntensity= InitConfiguration.CollisionDamageIntensity;

            Settings.Init();

            GameRuntime.CurrentGlobals.Init();
            GameRuntime.CurrentGlobals.UsingAsset = TargetAsset;
            Instantiate(BackgroundObject);
        }
    }
    [Serializable]
    public class Configuration
    {
        public float CollisionDamageSpeedThreshold;
        public float CollisionDamageIntensity;
    }
}
