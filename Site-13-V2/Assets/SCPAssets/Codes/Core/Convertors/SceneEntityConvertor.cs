using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core.Convertors
{
    public class SceneEntityConvertor : BaseConvertor
    {
        public override void Init()
        {
            GameRuntime.CurrentLocals.CurrentDefaultController.Register(GetComponent<DamagableEntity>());
        }
    }
}
