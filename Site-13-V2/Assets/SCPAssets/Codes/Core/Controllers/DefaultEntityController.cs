using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class DefaultEntityController : EntityController
    {
        public override void Init()
        {
            base.Init();
            Parent.RegisterRefresh(this);
            GameRuntime.CurrentLocals.CurrentDefaultController = this;
        }
    }
}
