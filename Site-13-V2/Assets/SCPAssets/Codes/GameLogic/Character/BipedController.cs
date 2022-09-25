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
        public bool UseControlledBehaviorWorkflow;
        public CharacterController CC;
        public Bag CurrentBag;
        public List<Renderer> ShieldedRenderers = new List<Renderer>();
        public Animator ControlledAnimator;
        public List<AnimationCollection> AnimationCollections = new List<AnimationCollection>();
        public Transform WeaponHand;
        public Transform Weapon1;
        public ActionLock MovementLock = new ActionLock();
        public ActionLock CombatLock = new ActionLock();
        public int FireID = 100;
        public int MeleeID = 101;
        public override void Move(Vector2 Movement, float DeltaTime)
        {
             
        }
        public override void HorizontalRotation(float Angle)
        {

        }
        public override void VerticalRotation(float Angle)
        {

        }
        public override void Melee()
        {

        }
        public override void UseEquipment()
        {

        }
        public override void ThrowGrenade()
        {

        }
        public override void Run()
        {

        }
        public override void CancelRun()
        {

        }
        public override void StartFire()
        {
            CurrentBag.Weapons[CurrentBag.CurrentWeapon].Fire();
        }
        public override void CancelFire()
        {
            CurrentBag.Weapons[CurrentBag.CurrentWeapon].Unfire();
        }
        public override void Interact()
        {

        }
        public void Update()
        {
            if(UseControlledBehaviorWorkflow)
            {
                return;
            }
            {
                float DT = Time.deltaTime;
                float UDT = Time.unscaledDeltaTime;
                OnFrame(DT, UDT);
            }
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (UseControlledBehaviorWorkflow)
            {
                OnFrame(DeltaTime, UnscaledDeltaTime);
            }
        }
        public void OnFrame(float DT,float UDT)
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
