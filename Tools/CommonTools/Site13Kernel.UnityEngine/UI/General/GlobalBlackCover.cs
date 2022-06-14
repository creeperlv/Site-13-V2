using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace Site13Kernel.UI.General
{
    public class GlobalBlackCover : ControlledBehavior
    {
        public static GlobalBlackCover Instance;
        public CanvasGroup ControlledBlackCover;
        public override void Init()
        {
            Instance = this;
        }
        public float PreDuration;
        public float MidDuration;
        public float PostDuration;
        public List<Action> ReachMid = new List<Action>();
        public bool isMidInvoked = false;
        public bool isPostInvoked = false;
        public List<Action> PostAction = new List<Action>();
        public float TimeD;
        public int Stage = -1;
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (Instance.Stage != -1)
            {
                switch (Stage)
                {
                    case 0:
                        {
                            ControlledBlackCover.alpha += UnscaledDeltaTime * (1 / PreDuration);
                            if (TimeD >= PreDuration)
                            {
                                foreach (var item in ReachMid)
                                {
                                    item();
                                }
                                isMidInvoked = true;
                                Stage = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            if(TimeD>PreDuration+MidDuration)
                            {
                                Stage = 2;
                            }
                        }
                        break;
                    case 2:
                        {
                            ControlledBlackCover.alpha -= UnscaledDeltaTime * (1 / PreDuration); 
                            if (TimeD >= PreDuration + MidDuration+PostDuration)
                            {
                                ControlledBlackCover.gameObject.SetActive(false);
                                foreach (var item in PostAction)
                                {
                                    item();
                                }
                                isPostInvoked = true;
                                Stage = -1;
                            }
                        }
                        break;
                    default:
                        break;
                }
                TimeD += UnscaledDeltaTime;
            }
        }
        public static void RequestShowCover(float PreDuration, float MiddleDuration, float PostDuration, Action ReachMid = null, Action PostAction = null)
        {
            if (Instance.Stage == -1)
            {
                Instance.ControlledBlackCover.gameObject.SetActive(true);
                Instance.Stage = 0;
                Instance.TimeD = 0;
                Instance.PreDuration = PreDuration;
            }
            if (Instance.Stage <= 1)
            {
                Instance.PostDuration = Mathf.Max(Instance.MidDuration, PostDuration);
            }
            Instance.MidDuration = Mathf.Max(Instance.MidDuration, MiddleDuration);
            if (ReachMid != null)
            {
                if (Instance.isMidInvoked)
                {
                    ReachMid();
                }
                else Instance.ReachMid.Add(ReachMid);
            }

            if (PostAction != null)
            {
                if (Instance.isPostInvoked)
                {
                    PostAction();
                }
                else Instance.ReachMid.Add(PostAction);
            }

        }
    }
}
