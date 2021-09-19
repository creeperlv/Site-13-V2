using Site13Kernel.Core.Convertors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public class ConvertorController : ControlledBehavior
    {
        public List<BaseConvertor> Convertors = new List<BaseConvertor>();
        public override void Init()
        {
            foreach (var item in Convertors)
            {
                item.Init();
            }
        }
    }
}
