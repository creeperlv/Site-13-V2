using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data
{
    [Serializable]
    public class BaseGrenade
    {
        public int GrenadeHashCode;
        public int EffectHashCode;
        public ExplosionDefinition Explosion;
        public float DetonationDuration;
    }
}
