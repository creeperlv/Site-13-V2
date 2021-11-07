using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class SimulatedRigidBodyOverCharacterController : ControlledBehavior
    {
        public CharacterController ControlledCharacterController;

        public List<Vector3> Forces = new List<Vector3>();
        public List<float> Force_LifeTime = new List<float>();

        public float Drag;
        public float Mass;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearForces()
        {
            Forces.Clear();
            Force_LifeTime.Clear();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddForce(Vector3 Force)
        {
            Forces.Add(Force);
            Force_LifeTime.Add(Force.magnitude / (Drag * Mass));
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            var DragForce = Drag * Mass;
            for (int i = 0; i < Forces.Count; i++)
            {
                ControlledCharacterController.Move(Forces[i] * DeltaTime);
                Force_LifeTime[i] -= DragForce * DeltaTime;
                if (Force_LifeTime[i] <= 0)
                {
                    Forces.RemoveAt(i);
                    Force_LifeTime.RemoveAt(i);
                }
            }
        }
    }
}
