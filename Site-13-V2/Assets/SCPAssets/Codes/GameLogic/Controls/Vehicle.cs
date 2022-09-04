using Site13Kernel.Core;
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
        
        bool RunByControlled;
        void Start()
        {
        
        }

        void Update()
        {
            if(!RunByControlled)
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
}
