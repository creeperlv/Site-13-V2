using Site13Kernel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace Site13Kernel.GameLogic
{


    public class BulletEmitter : MonoBehaviour
    {
        public int TargetBullet;
        public List<EmitPoint> Emitters;
        [Serializable]
        public class EmitPoint
        {
            public Transform Point;
            public float Drift;
            public float TimeD;
            public float Duration;
        }
        // Update is called once per frame
        void Update()
        {
            float DT = Time.deltaTime;
            foreach (var item in Emitters)
            {
                EmitFrame(item, DT);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EmitFrame(EmitPoint EP, float DeltaTime, bool isRight = false)
        {
            if (EP.TimeD > EP.Duration + EP.Drift)
            {
                GameRuntime.CurrentGlobals.CurrentBulletSystem.AddBullet(TargetBullet, EP.Point.position, EP.Point.rotation);
                EP.TimeD = EP.Drift;
            }
            EP.TimeD += DeltaTime;
        }
    }

}