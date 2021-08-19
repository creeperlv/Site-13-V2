using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic
{
    public class ControlledWeapon : ControlledBehavior
    {
        public Weapon Weapon;
        public Animator ControlledAnimator;
        public string TakeOut="TakeOut";
        public string Combat="Combat";
        public string Reload="Reload";
        public float MaxRecoil=2;
        public float RecoilRecoverSpeed=5;
        public GameObject Projectile;
        public void Fire()
        {

        }
        public override void Refresh(float DeltaTime)
        {

        }
    }
    public class Weapon
    {
        public string WeaponID;
        public float MaxCapacity;
        public float CurrentBackup;
        public float CurrentMagazine;
    }
}
