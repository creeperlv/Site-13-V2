using Site13Kernel.Core;
using System;

namespace Site13Kernel.GameLogic.Character
{
    [Serializable]
    public class TakeControl : ControlledBehavior
    {
        public static TakeControl Instance;
        public BipedController controller;
        public BipedEntity entity;
        public void OnEnable()
        {
            Instance = this;
            controller = GetComponentInChildren<BipedController>();
            entity = GetComponentInChildren<BipedEntity>();
        }
    }
}
