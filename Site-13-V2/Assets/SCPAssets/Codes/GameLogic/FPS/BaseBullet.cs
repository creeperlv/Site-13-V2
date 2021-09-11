using Site13Kernel.Core;
using Site13Kernel.GameLogic.FPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class BaseBullet : ControlledBehavior
    {
        public new BulletSystem Parent { get; set; }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {

        }
        private void OnCollisionEnter(Collision collision)
        {
            Hit(collision);
        }
        public virtual void Hit(Collision collision)
        {

        }
    }
}
