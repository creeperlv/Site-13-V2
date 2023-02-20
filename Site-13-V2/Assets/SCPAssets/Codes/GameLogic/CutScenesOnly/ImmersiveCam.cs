using Site13Kernel.Core.Controllers;
using Site13Kernel.GameLogic.Cameras;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class ImmersiveCam : ImmersiveCamBase
    {
        public override void OnEnable()
        {
            base.OnEnable();
            if (FPSController.Instance != null)
            {
                var t = FPSController.Instance.MainCam.transform;
                this.transform.position = t.position;
                this.transform.rotation = t.rotation;
            }
            //            Quaternion.
            //            Debug.Log((this.transform.rotation - FollowingTarget.rotation))
        }
       
    }
}
