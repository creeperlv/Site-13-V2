using Site13Kernel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic
{
    public class GlobalBioController : ControlledController
    {
        public List<BioDefinition> Definitions=new List<BioDefinition>();   

    }
    [Serializable]
    public class BioDefinition
    {
        public string BioEntityID;
        public GameObject ProtoType;
    }
}
