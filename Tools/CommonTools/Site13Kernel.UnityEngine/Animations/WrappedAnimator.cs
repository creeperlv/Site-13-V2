﻿using System;
using UnityEngine;

namespace Site13Kernel.Animations
{
    [Serializable]
    public class WrappedAnimator
    {
        public Animator ControlledAnimator;
        public string LastTrigger;
        public AnimationCollection CurrentCollection;
        public float AccumulativeTime = 0;
        public Site13AnimationClip CurrentClip = null;
        public Site13AnimationClip NextClip = null;
        public void UseAnimationCollection(AnimationCollection collection, bool ContinueLastTrigger = true)
        {
            CurrentCollection = collection;
            ControlledAnimator.runtimeAnimatorController = CurrentCollection.ControllerToUse;
            if (ContinueLastTrigger)
                ControlledAnimator.SetTrigger(LastTrigger);
        }
        public void AccumulativeTrigger(float DeltaTime)
        {
            if (CurrentClip == null) return;
            if (!CurrentClip.WaitUntilDone) return;
            if (CurrentClip.Trigger == "") return;
            AccumulativeTime += DeltaTime;
            if (NextClip == null) return;
            if (NextClip.Trigger == "") return;
            if (AccumulativeTime > CurrentClip.Length)
            {
                SetClip(NextClip);
            }
        }
        public void SetClip(Site13AnimationClip clip)
        {
            if (CurrentClip != null)
                if (CurrentClip.WaitUntilDone == true)
                    if (AccumulativeTime < CurrentClip.Length)
                    {
                        NextClip = clip;
                        return;
                    }
            AccumulativeTime = 0;
            CurrentClip = clip.Duplicate();
            NextClip = null;
            LastTrigger = clip.Trigger;
            ControlledAnimator.SetTrigger(LastTrigger);
        }

        public void ReTrigger()
        {
            if (CurrentCollection != null)
            {
                var __t = CurrentCollection.ObtainAnimationTrigger(LastTrigger);
                SetClip(__t);
            
            }
            ControlledAnimator.SetTrigger(LastTrigger);
        }
        public void SetTrigger(Site13AnimationClip site13AnimationClip)
        {
            SetClip(site13AnimationClip);
        }
        public Site13AnimationClip SetTrigger(string TriggerName,float Length, bool WaitUntilDone)
        {
            var clip=new Site13AnimationClip { Trigger=TriggerName,Length= Length ,WaitUntilDone   =WaitUntilDone};
            SetClip(clip);
            return clip;
        }
        public Site13AnimationClip SetTrigger(string TriggerName)
        {
            if (LastTrigger == TriggerName) return null;
            if (CurrentCollection != null)
            {
                var __t = CurrentCollection.ObtainAnimationTrigger(TriggerName);
                SetClip(__t);
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
