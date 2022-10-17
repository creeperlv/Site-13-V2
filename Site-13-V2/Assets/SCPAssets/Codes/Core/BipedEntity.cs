using Site13Kernel.GameLogic.Character;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class BipedEntity : BioEntity
    {
        public bool isPlayer = false;
        #region Weapon Stack
        public Transform WeaponHand;
        public Transform Weapon1;
        public Transform Weapon2;
        #endregion
        public Bag EntityBag;
        public Site13Event OnSwapWeapon = new Site13Event();
        //    public override 
        public override void Init()
        {
            base.Init();
            EntityBag.OnObtainWeapon.Add((w) =>
            {
                if (EntityBag.Weapons.Count < 2)
                {
                    EntityBag.CurrentWeapon++;
                }

                w.gameObject.transform.SetParent(WeaponHand);
            });
        }
    }
}
