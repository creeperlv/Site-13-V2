using Site13Kernel.Core;
using Site13Kernel.Data;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class WeaponLayerCamera:ControlledBehavior
    {
        public static WeaponLayerCamera Instance;
        public Camera RealCamera;
        public float MinFOV=40;
        public float MaxFOV=55;
        public float MainCamFOVL=60;
        public float MainCamFOVR=120;
        public void Start() { Instance = this; }
        public void OnEnable() { Instance = this; }
        public void Update()
        {
            RealCamera.fieldOfView = Mathf.Lerp(MinFOV, MaxFOV, Mathf.InverseLerp(MainCamFOVL, MainCamFOVR, Settings.CurrentSettings.FOV));
        }
    }
}
