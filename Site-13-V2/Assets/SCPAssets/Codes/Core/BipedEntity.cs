using Site13Kernel.Data;
using Site13Kernel.GameLogic.Character;
using Site13Kernel.GameLogic.Firefight;
using Site13Kernel.Utilities;
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
        public bool OverrideFirePoint;
        public Transform FirePoint;
        #endregion
        public Bag EntityBag;
        public Site13Event OnSwapWeapon = new Site13Event();
        public bool SelfRun = false;
        public void Start()
        {
            if (SelfRun) { Init(); }
        }
        public void Update()
        {
            if (SelfRun)
            {
                var d = Time.deltaTime;
                var ud = Time.unscaledDeltaTime;
                Refresh(d, ud);
            }
        }
        //    public override 
        public override void Init()
        {
            base.Init();
            EntityBag.OnObtainWeapon.Add((w) =>
            {
                if (EntityBag.Weapons.Count < 2)
                {
                    if (EntityBag.Weapons.Count == 1)
                        EntityBag.CurrentWeapon = 1;
                }
                else
                {
                    EntityBag.DropWeapon(EntityBag.Weapons[EntityBag.CurrentWeapon]);
                }
                EntityBag.Weapons.Add(w);
                if (OverrideFirePoint)
                {
                    w.CurrentFirePoint = FirePoint;
                }
                else
                {
                    w.CurrentFirePoint = w.FirePoint;
                }
                w.gameObject.transform.SetParent(WeaponHand);
                w.Holder = this;
                w.gameObject.transform.localPosition = Vector3.zero;

                w.gameObject.transform.localRotation = Quaternion.identity;
                w.ApplyObjectStatus(false);
                w.gameObject.transform.localScale = Vector3.one;
                ObjectGenerator.SetLayerForChildren(w.gameObject, WeaponHand.gameObject.layer);
                OnSwapWeapon.Invoke();
            });
            EntityBag.OnDropWeapon.Add((w) =>
            {
                w.Holder = null;
                w.ApplyObjectStatus(true);
                w.isHoldByPlayer = false;
                w.OnSingleFire.Clear();
                w.gameObject.transform.SetParent(WeaponPool.CurrentPool.transform);
            });
        }
    }
}
