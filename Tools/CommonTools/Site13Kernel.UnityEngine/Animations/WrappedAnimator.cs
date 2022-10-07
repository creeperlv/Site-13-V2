using System;
using UnityEngine;

namespace Site13Kernel.Animations
{
    [Serializable]
    public class WrappedAnimator
    {
        public Animator ControlledAnimator;
        public string LastTrigger;
        public void SetTrigger(string TriggerName)
        {
            if (LastTrigger == TriggerName) return;
            ControlledAnimator.SetTrigger(TriggerName);
            LastTrigger = TriggerName;
        }
        public void ForceSetTrigger(string TriggerName)
        {
            ControlledAnimator.SetTrigger(TriggerName);
            LastTrigger = TriggerName;
        }
    }
}
