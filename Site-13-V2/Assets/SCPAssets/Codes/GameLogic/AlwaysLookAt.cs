using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace Site13Kernel.GameLogic
{
    public class AlwaysLookAt : ControlledBehavior
    {
        public Transform Target;
        public bool UseGlobalTargets;
        public bool UseCamPosTarget=false;
        public bool __Registered = false;
        public override void Init()
        {
            Parent.RegisterRefresh(this);
            __Registered = true;
        }
        //private void Update()
        //{
        //    if (__Registered) return;
        //    Set();
        //}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Set()
        {
            if (UseGlobalTargets)
            {
                //if (EyeTarget.Instance != null)
                {
                    this.transform.LookAt(EyeTarget.Instance.ThisTransform);
                }
                if(UseCamPosTarget)
                if (CamPosTarget.Instance != null)
                {
                    this.transform.position = CamPosTarget.Instance.ThisTransform.position;
                }
            }
            else
            {
                this.transform.LookAt(Target);
            }
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            Set();
        }
    }

}