using Site13Kernel.GameLogic.Effects;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Data
{
    /// <summary>
    /// Useful for using inspector.
    /// </summary>
    [Serializable]
    public class EffectDefinition
    {
        public int HashCode;
        public GameObject Effect;
    }
}
