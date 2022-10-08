using System;
using UnityEngine;

namespace Site13Kernel.Animations
{
    [Serializable]
    public class WrappedAnimator
    {
        public Animator ControlledAnimator;
        public string LastTrigger;
        public AnimationCollection CurrentCollection;
        public void UseAnimationCollection(AnimationCollection collection)
        {
            CurrentCollection = collection;
            ControlledAnimator.runtimeAnimatorController = CurrentCollection.ControllerToUse;
            ControlledAnimator.SetTrigger(LastTrigger);
        }
        public Site13AnimationClip SetTrigger(string TriggerName)
        {
            if (LastTrigger == TriggerName) return null;
            if (CurrentCollection != null)
            {
                var __t = CurrentCollection.ObtainAnimationTrigger(TriggerName);
                LastTrigger = __t.Trigger;
                ControlledAnimator.SetTrigger(LastTrigger);
                return __t;
            }
            ControlledAnimator.SetTrigger(TriggerName);
            LastTrigger = TriggerName;
            return new Site13AnimationClip
            {
                Trigger = LastTrigger,
                Length = -1
            };
        }
        public void ForceSetTrigger(string TriggerName)
        {
            ControlledAnimator.SetTrigger(TriggerName);
            LastTrigger = TriggerName;
        }
    }
}
