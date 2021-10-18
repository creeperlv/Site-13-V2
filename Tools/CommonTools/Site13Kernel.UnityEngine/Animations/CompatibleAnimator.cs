using CLUNL.Data.Serializables.CheckpointSystem;
using CLUNL.Data.Serializables.CheckpointSystem.Types;
using Site13Kernel.Core;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Animations
{
    public class CompatibleAnimator : PropertiedControlledBehavior, ICheckpointData
    {
        [InspectorName("For Save Use")]
        public string Name;
        [InspectorName("Compatible Animator")]
        public CompatibleAnimatorWorkflow Workflow;
        public Animator ControlledAnimator;
        public Animation ControlledAnimation;
        public List<CompatibleAnimationClip> AnimationClips;
        public int CurrentClip;
        public int DefaultClip;
        public override void Init()
        {
            if (DefaultClip != -1)
            {
                SetAnimation(DefaultClip, true);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetTime()
        {
            switch (Workflow)
            {
                case CompatibleAnimatorWorkflow.Animator:
                    return ControlledAnimator.playbackTime;
                case CompatibleAnimatorWorkflow.Animation:

                    break;
                default:
                    break;
            }
            return 0;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTime(float T)
        {
            switch (Workflow)
            {
                case CompatibleAnimatorWorkflow.Animator:
                    ControlledAnimator.playbackTime = T;
                    break;
                case CompatibleAnimatorWorkflow.Animation:

                    break;
                default:
                    break;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAnimation(int HashCode, bool Force = false)
        {
            foreach (var item in AnimationClips)
            {
                if (item.HashCode == HashCode)
                {
                    if (!item.isIgnoredApplication)
                        switch (Workflow)
                        {
                            case CompatibleAnimatorWorkflow.Animator:
                                {
                                    if (Force)
                                    {
                                        ControlledAnimator.Play(item.AlternativeStateName);
                                    }
                                    else
                                    {

                                        ControlledAnimator.SetTrigger(item.AlternativeTrigger);
                                    }
                                }
                                break;
                            case CompatibleAnimatorWorkflow.Animation:
                                {
                                    ControlledAnimation.Stop();
                                    ControlledAnimation.clip = item.AnimationClip;
                                    ControlledAnimation.Play();
                                }
                                break;
                            default:
                                break;
                        }
                }
            }
            CurrentClip = HashCode;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Play()
        {
            switch (Workflow)
            {
                case CompatibleAnimatorWorkflow.Animator:
                    ControlledAnimator.StartPlayback();
                    break;
                case CompatibleAnimatorWorkflow.Animation:
                    ControlledAnimation.Play();
                    break;
                default:
                    break;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Pause()
        {
            switch (Workflow)
            {
                case CompatibleAnimatorWorkflow.Animator:
                    ControlledAnimator.StopPlayback();
                    break;
                case CompatibleAnimatorWorkflow.Animation:
                    ControlledAnimation.Stop();
                    break;
                default:
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetName()
        {
            return Name;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<object> Save()
        {
            return new List<object> { CurrentClip, GetTime() };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Load(List<object> data)
        {
            CurrentClip = ((IntNumber)data[0]).Data;
            SetTime(((FloatNumber)data[1]).Data);
            SetAnimation(CurrentClip, true);
        }
    }
}
