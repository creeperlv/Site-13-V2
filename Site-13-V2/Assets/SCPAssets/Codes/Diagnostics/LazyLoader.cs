using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Diagnostics
{
    public class LazyLoader : MonoBehaviour
    {
        public List<LazyLoadControlledBehavior> Behaviors = new List<LazyLoadControlledBehavior>();
        void Start()
        {
        
        }
        int FrameCount = 0;
        float TimeC;
        // Update is called once per frame
        void Update()
        {
            TimeC += Time.unscaledDeltaTime;
            FrameCount++;
            foreach (var item in Behaviors)
            {
                if (item.Loaded) continue;
                if (item.FrameCount > -1)
                {
                    if (FrameCount > item.FrameCount)
                    {
                        item.Loaded = true;
                        item.Behavior.Init();
                    }
                }
                else
                {
                    if (TimeC > item.DelayTime)
                    {
                        item.Loaded = true;
                        item.Behavior.Init();
                    }
                }
            }
        }
    }
}
