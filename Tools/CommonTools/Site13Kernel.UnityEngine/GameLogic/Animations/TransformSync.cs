using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.GameLogic.Animations
{
    public class TransformSync : ControlledBehavior
    {
        public List<TrackedTransforms> transforms = new List<TrackedTransforms>();
        public void Update()
        {
            foreach (var item in transforms)
            {
                item.Target.position = item.Source.position;
                item.Target.rotation = item.Source.rotation;
            }
        }
    }
    [Serializable]
    public class TrackedTransforms
    {
        public Transform Source;
        public Transform Target;
    }
}
