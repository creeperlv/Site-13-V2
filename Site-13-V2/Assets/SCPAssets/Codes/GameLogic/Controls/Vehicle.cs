using Site13Kernel.Core;
using Site13Kernel.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Controls
{
    public class Vehicle : ControlledBehavior
    {
        public List<Transform> Wheels;
        public float F_B_Motivation;
        public float L_R_Motivation;
        public List<KVPair<EventTrigger, VehicleSeat>> Entrances;
        public bool InitOnStart;
        public bool RunByControlled;
        void Start()
        {
            if (InitOnStart)
            {
                __init();
            }
        }
        public override void Init()
        {
            if (!InitOnStart) __init();
        }
        void __init()
        {

        }
        void Update()
        {
            if (!RunByControlled)
                OnFrame(Time.deltaTime);
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            OnFrame(DeltaTime);
        }
        void OnFrame(float DT)
        {

        }
    }
    [Serializable]
    public class VehicleSeat
    {
        public Transform SeatTransform;
        public string NormalTrigger;
        public string HijackTrigger;
    }
}
