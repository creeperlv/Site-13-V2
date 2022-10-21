using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Utilities;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class MainCamera : ControlledBehavior
    {
        public static MainCamera Instance;
        public Camera RealCamera;
        public float CurrentScale;
        public float TargetScale;
        public float Speed=10;
        public void Start()
        {
            Instance = this;
        }
        public void OnEnable()
        {
            Instance = this;
        }
        public void SetViewScale(float scale)
        {
            RealCamera.fieldOfView = Settings.CurrentSettings.FOV / scale;
        }
        public void Update()
        {
            CurrentScale = MathUtilities.SmoothClose(CurrentScale, TargetScale, Time.deltaTime*Speed);
            SetViewScale(CurrentScale);
        }
    }
}
