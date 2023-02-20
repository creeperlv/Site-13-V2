using Site13Kernel.Utilities;
using UnityEngine;

namespace Site13Kernel.GameLogic.Cameras
{
    public class ImmersiveCamBase : MonoBehaviour
    {
        public static ImmersiveCamBase GlobalCam;
        public bool FollowCamPosTarget;
        public Transform FollowingTarget;
        public float Speed = 1;
        public float RotationSpeed = 1;
        public bool SmoothFollow = true;
        public bool FollowRotation = true;
        void Start()
        {
            GlobalCam = this;

        }
        public virtual void OnEnable()
        {
            GlobalCam = this;
           
            //            Quaternion.
            //            Debug.Log((this.transform.rotation - FollowingTarget.rotation))
        }
        void Follow(Transform FollowingTarget, float dt)
        {
            if (SmoothFollow)
            {
                this.transform.position = MathUtilities.SmoothClose(this.transform.position, FollowingTarget.position, dt * Speed);
                if(FollowRotation)
                    this.transform.rotation = MathUtilities.SmoothClose(this.transform.rotation, FollowingTarget.rotation, dt * RotationSpeed);
                return;
            }
            this.transform.position = FollowingTarget.position;
                if(FollowRotation)
            this.transform.rotation = FollowingTarget.rotation;
        }
        void Update()
        {
            var dt = Time.deltaTime;
            if (FollowCamPosTarget)
            {
                if (CamPosTargetBase.Instance != null)
                    Follow(CamPosTargetBase.Instance.ThisTransform, dt);
                return;
            }
            else
                if (FollowingTarget != null)
                Follow(FollowingTarget, dt);
        }
    }
}
