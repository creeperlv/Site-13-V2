using Site13Kernel.Core.TagSystem;
using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Character
{
    public class AnimatedCharacter : AttachableComponent
    {
        public Transform Root;
        public Animator Animator;
        public KVList<string, BipedMotion> MotionMap;
        public Dictionary<string, BipedMotion> _MotionMap;
    }
}
