using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.Controls;
using Site13Kernel.GameLogic.FPS;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

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
        public float RunningJumpHeight = 1f;
        public float MouseVerticalIntensity = 1f;
        public float MouseHoriztonalIntensity = 1f;
        public float MaxV = 50;
        public float MinV = -50;
        public float WalkSpeed = 5f;
        public float MoveFriction = 9.8f;
        float MH;
        float MV;
        float VR;
        float HR;
        Vector3 _MOVE;
        public override void Move(Vector2 Movement, float DeltaTime)
        {
            MV = Movement.x;
            MH = Movement.y;
        }
        public override void HorizontalRotation(float Angle)
        {
            HR = Angle;
        }
        public override void VerticalRotation(float Angle)
        {
            VR = Angle;
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
            if (UseControlledBehaviorWorkflow)
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
        void Rotation(float DT)
        {

            HorizontalTransform.Rotate(new Vector3(0, HR* MouseHoriztonalIntensity * DT * Data.Settings.CurrentSettings.MouseSensibly, 0));
            var Head_V = VR * MouseHoriztonalIntensity * DT * Data.Settings.CurrentSettings.MouseSensibly;
            var ea = VerticalTransform.localEulerAngles;
            ea.x += Head_V;
            if (ea.x < 180)
            {
                ea.x = Mathf.Clamp(ea.x, MinV, MaxV);
            }
            else
            {
                ea.x = Mathf.Clamp(ea.x, 360 + MinV, 360);

            }
            VerticalTransform.localEulerAngles = ea;
        }
        void Move(float DT)
        {
            if (MV == 0 && MH == 0)
            {
                _MOVE -= _MOVE * MoveFriction * DT;
                if (_MOVE.magnitude <= 0.03f)
                {
                    _MOVE = Vector3.zero;
                }
            }
            else
            {
                _MOVE = CC.transform.right * (MH * math.sqrt(1 - (MV * MV) * .5f)) + CC.transform.forward * (MV * math.sqrt(1 - (MH * MH) * .5f));
            }
            CC.Move(_MOVE * DT);
        }
        public void OnFrame(float DT, float UDT)
        {
            {
                Rotation(DT);
                Move(DT);
            }
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
