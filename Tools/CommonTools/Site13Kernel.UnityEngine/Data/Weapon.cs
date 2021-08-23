using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data
{
    [Serializable]
    public class Weapon
    {
        public string WeaponID;
        public float MaxCapacity;
        public float CurrentBackup;
        public float CurrentMagazine;
        public WeaponMode WeaponMode;
    }
    public enum WeaponMode
    {
        SingleFire, Brust3Bullet, FullAuto, StB, BtF, AllMode
    }
}
