using Site13Kernel.Animations;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.Controls;
using Site13Kernel.GameLogic.FPS;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace Site13Kernel.GameLogic.Character
{
    public class BipedController : BasicController
    {
        public string BipedID;
        public BipedEntity Entity;
        public bool UseControlledBehaviorWorkflow;
        public CharacterController CC;
        public List<Renderer> ShieldedRenderers = new List<Renderer>();
        public WrappedAnimator ControlledAnimator;
        public List<AnimationCollection> AnimationCollections = new List<AnimationCollection>();
        public Transform WeaponHand;
        public Transform Weapon1;
        public ActionLock MovementLock = new ActionLock();
        public ActionLock CombatLock = new ActionLock();
        public MoveState MoveState = MoveState.Walk;
        public int FireID = 100;
        public int MeleeID = 101;
        public float RunningJumpHeight = 1f;
        public float MouseVerticalIntensity = 1f;
        public float MouseHoriztonalIntensity = 1f;
        public float MaxV = 50;
        public float MinV = -50;
        public float WalkSpeed = 5f;
        public float MoveFriction = 9.8f;
        public float SprintMultiplyer = 1.5f;
        public float SpeedMultiplyer = 1;
        public float Gravity = 9.8f;
        public float JumpForce = 10f;
        public float RunningJumpForce = 15f;
        public float DropThreshold = 0.5f;
        float MH;
        float MV;
        float VR;
        float HR;
        Vector3 _MOVE;
        public AnimationCollection CurrentCollection;
        void __init()
        {
            //CurrentCollection=
        }
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
        bool __try_jump = false;
        public override void Jump()
        {
            __try_jump = true;
        }
        public override void UseEquipment()
        {

        }
        public override void ThrowGrenade()
        {

        }
        public override void Run()
        {
            //ControlledAnimator.SetTrigger("FSMR-SM2");
            //ControlledAnimator.SetTrigger("Run");
            SpeedMultiplyer = SprintMultiplyer;
        }
        public override void CancelRun()
        {
            //ControlledAnimator.SetTrigger("Idle");
            SpeedMultiplyer = 1;
        }
        public override void StartFire()
        {
            Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon].Fire();
        }
        public override void CancelFire()
        {
            Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon].Unfire();
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

            HorizontalTransform.Rotate(new Vector3(0, HR * MouseHoriztonalIntensity * DT * Data.Settings.CurrentSettings.MouseSensibly, 0));
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
        public float DropTime= 0;
        void Move(float DT)
        {
            if (CC.isGrounded)
            {
                if (MV == 0 && MH == 0)
                {
                    _MOVE -= _MOVE * MoveFriction * DT;
                    if (_MOVE.magnitude <= 0.03f)
                    {
                        _MOVE = Vector3.zero;

                        _MOVE.y = -Gravity;
                    }
                }
                else
                {
                    var m = (CC.transform.right * (MH * math.sqrt(1 - (MV * MV) * .5f)) + CC.transform.forward * (MV * math.sqrt(1 - (MH * MH) * .5f)));
                    m *= WalkSpeed;
                    m *= SpeedMultiplyer;
                    //_MOVE.y = -Gravity;
                    m.y = _MOVE.y;
                    _MOVE = m;
                }
                if (__try_jump)
                {
                    _MOVE.y = (SpeedMultiplyer == SprintMultiplyer ? RunningJumpForce : JumpForce);
                    __try_jump = false;
                }
                else
                {

                }
            }
            else
            {
                
                _MOVE.y -= Gravity * DT;
                __try_jump = false;
            }

            if (CC.isGrounded)
            {
                if (DropTime >= DropThreshold)
                {
                    {
                        ControlledAnimator.SetTrigger("HeavyLand");
                    }
                }
                DropTime = 0;
            }
            else
            {
                if (DropTime >=0.1f)
                {
                    ControlledAnimator.SetTrigger("Idle");
                }
                DropTime += DT;
            }
            CC.Move(_MOVE * DT);

            if ((new Vector2(CC.velocity.x, CC.velocity.z)).sqrMagnitude > 0.1f)
            {
                if (CC.isGrounded)
                if (MH!=0||MV!=0)
                    switch (ControlledAnimator.LastTrigger)
                    {
                        case "Run":
                        case "Walk":
                        case "Idle":
                        case "HeavyLand":
                        case "":
                            {
                                if (SpeedMultiplyer == SprintMultiplyer)
                                {
                                    //Running.
                                    ControlledAnimator.SetTrigger("Run");
                                }
                                else
                                {
                                    ControlledAnimator.SetTrigger("Walk");
                                }
                            }
                            break;
                        default:
                            break;
                    }
            }
            else
            {
                switch (ControlledAnimator.LastTrigger)
                {
                    case "Run":
                    case "Walk":
                        ControlledAnimator.SetTrigger("Idle");
                        break;
                    default:
                        break;
                }
            }
        }
        public void OnFrame(float DT, float UDT)
        {
            {
                Rotation(DT);
                Move(DT);
            }
        }
    }
    [Serializable]
    public class Bag
    {
        public List<GenericWeapon> Weapons = new List<GenericWeapon>();
        public int CurrentWeapon;
        public List<GrenadeItem> Grenades = new List<GrenadeItem>();
        public int CurrentGrenade;
        public Site13Event OnUseWeapon = new Site13Event();
        public Site13Event<GenericWeapon> OnObtainWeapon = new Site13Event<GenericWeapon>();
        public Site13Event<GenericWeapon> OnDropWeapon = new Site13Event<GenericWeapon>();
        public void TryObatinWeapon(GenericWeapon GW)
        {

        }
        public void UseWeeapon(int Weapon)
        {
            if (CurrentWeapon == Weapon) return;
            CurrentWeapon = Weapon;
            OnUseWeapon.Invoke();
        }
    }
}
