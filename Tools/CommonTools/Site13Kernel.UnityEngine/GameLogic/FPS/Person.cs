using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.GameLogic.FPS
{
    public class Person:ControlledBehavior
    {
        public Site13Event OnHitOther=new Site13Event();
        public Site13Event OnHitByOther=new Site13Event();
        public Site13Event OnKillOther=new Site13Event();
        public Site13Event OnDie=new Site13Event();
    }
}
