using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.FPS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic
{
    public class LogoSequenceWeaponEmitter : MonoBehaviour
    {
        public PrefabReference PlasmaBullet;
        public PrefabReference HitBullet;
        public List<EmitPoint> LeftEmitters;
        public List<EmitPoint> RightEmitters;
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
            foreach (var item in LeftEmitters)
            {
                EmitFrame(item, DT);
            }
            foreach (var item in RightEmitters)
            {
                EmitFrame(item, DT, true);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EmitFrame(EmitPoint EP, float DeltaTime, bool isRight = false)
        {
            if (EP.TimeD > EP.Duration + EP.Drift)
            {
                if (!isRight)
                    GameRuntime.CurrentGlobals.CurrentBulletSystem.AddBullet(PlasmaBullet, EP.Point.position, EP.Point.rotation);
                else
                    GameRuntime.CurrentGlobals.CurrentBulletSystem.AddBullet(HitBullet, EP.Point.position, EP.Point.rotation);
                EP.TimeD = EP.Drift;
            }
            EP.TimeD += DeltaTime;
        }
    }

}
