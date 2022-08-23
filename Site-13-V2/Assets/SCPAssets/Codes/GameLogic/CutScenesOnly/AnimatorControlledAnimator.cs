using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.CutScenesOnly
{
    public class AnimatorControlledAnimator : MonoBehaviour
    {
        public int UsingTrigger;
        public Animator ControlledAnimator;
        public List<KVPair<int, string>> Triggers;
        int __LastTrigger = 0;
        void Update()
        {
            if (UsingTrigger != __LastTrigger)
            {
                foreach (var item in Triggers)
                {
                    if (item.Key == UsingTrigger)
                    {
                        ControlledAnimator.SetTrigger(item.Value);
                    }
                }
                __LastTrigger = UsingTrigger;
            }
        }
    }
}
