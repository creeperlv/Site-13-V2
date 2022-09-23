using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.Controls;
using Site13Kernel.GameLogic.FPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Character
{
    public class BipedController : BasicController
    {
        public BioEntity Entity;
        public Bag CurrentBag;
        public List<Renderer> ShieldedRenderers = new List<Renderer>();
        public Animator ControlledAnimator;
        public List<AnimationCollection> AnimationCollections = new List<AnimationCollection>();
        public Transform WeaponHand;
        public Transform Weapon1;
        public void Melee()
        {

        }
        public void UseEquipment()
        {

        }
        public void ThrowGrenade()
        {

        }
        public void Run()
        {

        }
        public void CancelRun()
        {

        }
        public void StartFire()
        {
            CurrentBag.Weapons[CurrentBag.CurrentWeapon].Fire();
        }
        public void CancelFire()
        {
            CurrentBag.Weapons[CurrentBag.CurrentWeapon].Unfire();
        }
        public void Interact()
        {

        }
    }

    public class Bag
    {
        public List<BasicWeapon> Weapons = new List<BasicWeapon>();
        public int CurrentWeapon;
        public List<GrenadeItem> Grenades = new List<GrenadeItem>();
        public int CurrentGrenade;
    }
}
