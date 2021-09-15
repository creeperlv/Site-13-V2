using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic
{
    public class BackgroundBehaviorInitializer : ControlledBehavior
    {
        public Transform BulletHolder;
        public static bool isInited = false;
        public override void Init()
        {
            if (isInited == true)
            {
                Destroy(Parent.gameObject);
                Parent.Interrupt(InterruptStage.Init);
                Parent.Interrupt(InterruptStage.Refresh);
                Parent.Interrupt(InterruptStage.FixedRefresh);
                throw new Exception("Background Object Only Allow One Instance");
                //return;
            }
            isInited = true;
            GameRuntime.BulletHolder = BulletHolder;
        } 
    }
}
