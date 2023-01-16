using Site13Kernel.Core.Controllers;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class ImmersiveCam : MonoBehaviour
    {
        public static ImmersiveCam GlobalCam;
        public bool FollowCamPosTarget;
        public Transform FollowingTarget;
        public float Speed = 1;
        public float RotationSpeed = 1;
        public bool SmoothFollow = true;
        void Start()
        {
            GlobalCam = this;

        }
        private void OnEnable()
        {
            GlobalCam = this;
            if (FPSController.Instance != null)
            {
                var t = FPSController.Instance.MainCam.transform;
                this.transform.position = t.position;
                this.transform.rotation = t.rotation;
            }
            //            Quaternion.
            //            Debug.Log((this.transform.rotation - FollowingTarget.rotation))
        }
        void Follow(Transform FollowingTarget, float dt)
        {
            if (SmoothFollow)
            {
                this.transform.position = MathUtilities.SmoothClose(this.transform.position, FollowingTarget.position, dt * Speed);
                this.transform.rotation = MathUtilities.SmoothClose(this.transform.rotation, FollowingTarget.rotation, dt * RotationSpeed);
                return;
            }
            this.transform.position = FollowingTarget.position;
            this.transform.rotation = FollowingTarget.rotation;
        }
        void Update()
        {
            var dt = Time.deltaTime;
            if (FollowCamPosTarget)
            {
                if (CamPosTarget.Instance != null)
                    Follow(CamPosTarget.Instance.ThisTransform, dt);
                return;
            }
            else
                if (FollowingTarget != null)
                Follow(FollowingTarget, dt);
        }
    }
}
