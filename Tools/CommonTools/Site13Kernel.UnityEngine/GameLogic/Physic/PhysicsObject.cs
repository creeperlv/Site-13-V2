using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.GameLogic.Physic
{
    public class PhysicalMaterial:ControlledBehavior
    {
        public int MaterialID;
    }
    public class PhysicsObject:MonoBehaviour
    {
        public PhysicalMaterial ReferneceMaterial;
        public Rigidbody body;
        public DamagableEntity Emitter;
        public void OnCollisionEnter(Collision collision)
        {
            var other=collision.collider.GetComponent<PhysicsObject>();
            if(other != null)
                if (body.velocity.magnitude > other.body.velocity.magnitude)
                {
                    other.Emitter = this.Emitter;
                }
        }
    }
}
