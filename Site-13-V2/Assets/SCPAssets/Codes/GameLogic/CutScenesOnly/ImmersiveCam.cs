using Site13Kernel.Core.Controllers;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class ImmersiveCam : MonoBehaviour
    {
        public bool FollowCamPosTarget;
        public Transform FollowingTarget;
        public float Speed=1;
        public float RotationSpeed=1;
        void Start()
        {
        
        }
        private void OnEnable()
        {
            var t=FPSController.Instance.MainCam.transform;
            this.transform.position = t.position;
            this.transform.rotation = t.rotation;
//            Quaternion.
//            Debug.Log((this.transform.rotation - FollowingTarget.rotation))
        }
        void Update()
        {
            var dt = Time.deltaTime;
            if(FollowCamPosTarget)
            {
                this.transform.position = MathUtilities.SmoothClose(this.transform.position, CamPosTarget.Instance.ThisTransform.position, dt * Speed);
                this.transform.rotation = MathUtilities.SmoothClose(this.transform.rotation, CamPosTarget.Instance.ThisTransform.rotation, dt * RotationSpeed);
                return;
            }
            this.transform.position=MathUtilities.SmoothClose(this.transform.position, FollowingTarget.position, dt* Speed);
            this.transform.rotation=MathUtilities.SmoothClose(this.transform.rotation, FollowingTarget.rotation, dt* RotationSpeed);
        }
    }
}
