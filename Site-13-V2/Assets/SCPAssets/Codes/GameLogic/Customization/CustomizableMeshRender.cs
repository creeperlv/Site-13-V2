using Site13Kernel.Data;
using System;
using UnityEngine;

namespace Site13Kernel.GameLogic.Customization
{
    [Serializable]
    public class CustomizableMeshRenderer
    {
        public MeshRenderer ControlledRenderer;
        public KVList<int, int> MaterialMap;
    }
}
