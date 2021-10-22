using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data
{
    [Serializable]
    public class BaseGrenade
    {
        public int GrenadeHashCode;
        public float Radius;
        public float Power;
        public float CentralDamage;
        public float DetonationDuration;
        public float ExistenceDuration;
    }
}
