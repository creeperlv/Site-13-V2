using Site13Kernel.Core.Controllers;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class ImmersiveCam : MonoBehaviour
    {
        public Transform FollowingTarget;
        public float Speed=1;
        void Start()
        {
        
        }
        private void OnEnable()
        {
            var t=FPSController.Instance.MainCam.transform;
            this.transform.position = t.position;
            this.transform.rotation = t.rotation;
        }
        void Update()
        {
            var dt = Time.deltaTime;
            this.transform.position=MathUtilities.SmoothClose(this.transform.position, FollowingTarget.position, dt* Speed);
            this.transform.rotation=MathUtilities.SmoothClose(this.transform.rotation, FollowingTarget.rotation, dt* Speed);
        }
    }
}
