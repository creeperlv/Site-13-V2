using Site13Kernel.Data.IO;
using Site13Kernel.GameLogic.FPS;
using System;

namespace Site13Kernel.Data
{
    [Serializable]
    public class Weapon : IPureData
    {
        public string WeaponID;
        public float MaxCapacity;
        public float MagazineCapacity;
        public float CurrentBackup;
        public float CurrentMagazine;
        public float PhysicsForce;
        public WeaponFireType WeaponFireType0;
        public WeaponFireType WeaponFireType1;// For 2-mode weapons.
        public Weapon Duplicate()
        {
            return new Weapon
            {
                WeaponID = WeaponID,
                MaxCapacity = MaxCapacity,
                MagazineCapacity = MagazineCapacity,
                CurrentBackup = CurrentBackup,
                CurrentMagazine = CurrentMagazine,
                PhysicsForce = PhysicsForce,
                WeaponFireType0 = WeaponFireType0,
                WeaponFireType1 = WeaponFireType1,
            };
        }
    }
}
