using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.Controls;
using Site13Kernel.GameLogic.FPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Character
{
    public class CharacterController : BasicController
    {
        public BioEntity Entity;
        public List<Renderer> ShieldedRenderers = new List<Renderer>();
        public Animator ControlledAnimator;
        public Transform HorizontalTransform;
        public Transform VerticalTransform;
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

        }
        public void CancelFire()
        {

        }
    }

    public class Bag
    {
        public List<BasicWeapon> Weapons = new List<BasicWeapon>();
        public List<GrenadeItem> Grenades = new List<GrenadeItem>();
    }
}
