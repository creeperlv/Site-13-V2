using Site13Kernel.GameLogic.Cameras;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class CamPosTarget : CamPosTargetBase
    {
        //public new Transform ThisTransform;
        void Start()
        {
            TakeControl();
        }
        //void TakeControl()
        //{
        //    Instance = this;
        //    ThisTransform = transform;
        //    if (ImmersiveCam.GlobalCam != null)
        //    {
        //        StartCoroutine(ResetCam());
        //    }
        //}
        private IEnumerator ResetCam()
        {
            ImmersiveCam.GlobalCam.enabled = false;
            yield return null;
            ImmersiveCam.GlobalCam.enabled = true;
        }
        private void OnEnable()
        {
            TakeControl();
        }
        //private void Update()
        //{
            
        //}
    }
}
