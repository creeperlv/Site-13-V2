using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Utilities;
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
        public Camera MainCamera;
        public float MinFOV=40;
        public float MaxFOV=55;
        public float MainCamFOVL=60;
        public float MainCamFOVR=120;
        public float CurrentScale=1;
        public float TargetScale=1;
        public float Speed = 15;
        public void Start() { Instance = this; }
        public void OnEnable() { Instance = this; }
        public void Update()
        {
            CurrentScale = MathUtilities.SmoothClose(CurrentScale, TargetScale, Time.deltaTime * Speed);
            RealCamera.fieldOfView = Mathf.Lerp(MinFOV, MaxFOV, Mathf.InverseLerp(MainCamFOVL, MainCamFOVR, Settings.CurrentSettings.FOV))/CurrentScale;
//            RealCamera.fieldOfView = Mathf.Lerp(MinFOV, MaxFOV, Mathf.InverseLerp(MainCamFOVL, MainCamFOVR, MainCamera.fieldOfView));
        }
    }
}
