using System;

namespace Site13Kernel.GameLogic.Firefight
{
    [Serializable]
    public class FirefightDefinition
    {
        public FirefightMode GameMode;
        public float TimeLength;
        public bool AllowWeaponSpawn;
    }
}
