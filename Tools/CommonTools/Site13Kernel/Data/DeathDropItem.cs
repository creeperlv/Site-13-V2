using System;

namespace Site13Kernel.Data
{
    [Serializable]
    public class DeathDropItem
    {
        public bool isWeapon = true;
        public PrefabReference ItemID;
        public float Probability = 1;
    }
}
