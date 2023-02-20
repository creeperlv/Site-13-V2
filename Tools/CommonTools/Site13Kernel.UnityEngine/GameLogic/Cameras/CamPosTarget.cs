using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.GameLogic.Cameras
{
    public class CamPosTargetBase : MonoBehaviour
    {
        public static CamPosTargetBase Instance;
        public Transform ThisTransform;
        void Start()
        {
            TakeControl();
        }
        public void SoftTakeControl()
        {
            Instance = this;
            ThisTransform = transform;
        }
        public void TakeControl()
        {
            Instance = this;
            ThisTransform = transform;
            if (ImmersiveCamBase.GlobalCam != null)
            {
                StartCoroutine(ResetCam());
            }
        }
        private IEnumerator ResetCam()
        {
            ImmersiveCamBase.GlobalCam.enabled = false;
            yield return null;
            ImmersiveCamBase.GlobalCam.enabled = true;
        }
        private void OnEnable()
        {
            TakeControl();
        }
    }
}
