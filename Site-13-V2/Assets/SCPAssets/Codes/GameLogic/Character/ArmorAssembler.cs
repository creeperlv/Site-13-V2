using Site13Kernel.Core;
using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class ArmorAssembler : ControlledBehavior
    {
        public List<KVPair<string, Transform>> ArmorPositions;
        public ArmorDescription DefaultArmor;
        public void Assemble(ArmorDescription description=null)
        {

        }
    }
}
