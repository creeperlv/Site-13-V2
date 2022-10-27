using Site13Kernel.Animations;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.GameLogic.Controls;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.GameLogic.Level;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.ProBuilder;
using UnityEngine.UIElements;

namespace Site13Kernel.GameLogic.Character
{
    public class BipedController : BasicController
    {
        public string BipedID;
        public ActiveInteractor Interactor;
        public BipedEntity Entity;
        public bool UseControlledBehaviorWorkflow;
        public CharacterController CC;
        public float NormalSkinWidth = 0.08f;
        public float CrouchSkinWidth = 0.008f;
        public WrappedAnimator ControlledAnimator;
        public float WalkFootStepMultiplier = 0.5f;
        public float SprintFootStepMultiplier = 0.75f;
        public float CrouchFootStepMultiplier = 0.15f;
        public float LandFootStepMultiplier = 0.85f;
        public float HeavyLandFootStepMultiplier = 0.9f;
        public float CurrentFootStepMultiplier = 0.5f;
        public AudioSource FootStepSoundSource;
        public ActionLock MovementLock = new ActionLock();
        public ActionLock CombatLock = new ActionLock();
        public float NormalCharacterHeight = 1.5f;
        public float NormalCharacterOffset_Y = 0.75f;
        public float CrouchCharacterHeight = 0.75f;
        public float CrouchCharacterOffset_Y = 0.325f;
        public float CharacterHeightChangeSpeed = 2f;
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
        public float CrouchMultiplyer = 1.5f;
        public float SpeedMultiplyer = 1;
        public float Gravity = 9.8f;
        public float JumpForce = 10f;
        public float RunningJumpForce = 15f;
        public float DropThreshold = 0.5f;
        public CamPosTarget CamPosTarget;
        public bool SmoothCamFollow = false;
        public bool isPlayer;
        public bool IsFPSBiped;
        float MH;
        float MV;
        float VR;
        float HR;
        public bool ALLOW_FIRE_FLAG_0 = true;   //SPRINT RUN LOCK
        public bool ALLOW_FIRE_FLAG_1 = true;  // PICK UP ACTION LOCK
        public bool ALLOW_FIRE_FLAG_2 = true;  // MELEE ACTION LOCK
        public bool ALLOW_FIRE_FLAG_3 = true;  // RELOAD LOCK
        Vector3 _MOVE;
        public AnimationCollection CurrentCollection;
        public AuxiliaryBipedControls ABC;
        public void Start()
        {
            if (UseControlledBehaviorWorkflow) return;
            __init();
        }
        public override void Init()
        {
            if (!UseControlledBehaviorWorkflow) return;
            __init();
        }
        void __init()
        {
            ControlledAnimator.ControlledAnimator.keepAnimatorControllerStateOnDisable = true;
            Entity.EntityBag.OnObtainWeapon.Add((w) =>
            {
                w.OnSingleFire.Add(() =>
                {
                    ControlledAnimator.SetTrigger("Fire");
                    ControlledAnimator.LastTrigger = "";
                });
                w.isHoldByPlayer = isPlayer;
                ApplyWeapon();
                StartCoroutine(PlayPickup());
            });
            Entity.OnSwapWeapon.Add(() =>
            {
                ApplyWeapon();
            });
        }
        IEnumerator PlayPickup()
        {
            ALLOW_FIRE_FLAG_1 = false;
            yield return null;
            yield return null;
            var col=ControlledAnimator.SetTrigger("Pickup");
            if (col.Length >= 0)
                yield return new WaitForSeconds(col.Length);
            else yield return null;
            ALLOW_FIRE_FLAG_1 = true;
        }
        void ApplyWeapon()
        {
            StartCoroutine(DelayedApplyWeapon());
        }
        IEnumerator DelayedApplyWeapon()
        {
            yield return null;

            GenericWeapon CurrentMain = Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon];
            GenericWeapon Side = null;
            if (Entity.EntityBag.Weapons.Count > 1)
            {
                Side = Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon == 1 ? 0 : 1];
            }
            ControlledAnimator.UseAnimationCollection(RuntimeAnimationResource.CachedResources[BipedID].Animations[CurrentMain.AnimationCollectionName], false);
            CurrentMain.transform.SetParent(Entity.WeaponHand);
            CurrentMain.transform.localPosition = Vector3.zero;
            CurrentMain.transform.localRotation = Quaternion.identity;
            CurrentMain.transform.localScale = Vector3.one;
            if (Side != null)
            {
                Side.transform.SetParent(Entity.Weapon1);
                Side.transform.localPosition = Vector3.zero;
                Side.transform.localRotation = Quaternion.identity;
                Side.transform.localScale = Vector3.one;
            }
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
        public override void Reload()
        {
            CancelRun();
            CancelZoom();
            if (!ALLOW_FIRE_FLAG_0) return;
            if (!ALLOW_FIRE_FLAG_1) return;
            if (!ALLOW_FIRE_FLAG_2) return;
            if (!ALLOW_FIRE_FLAG_3) return;
            StartCoroutine(ReloadProcess());
        }
        IEnumerator ReloadProcess()
        {
            if (Entity.EntityBag.Weapons.Count == 0)
            {
                yield break;
            }
            if (Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon].WeaponData.CurrentBackup <= 0) yield break;
            if (Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon].WeaponData.CurrentMagazine >=
                Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon].WeaponData.MagazineCapacity) yield break;
            ALLOW_FIRE_FLAG_3 = false;
            var clip = ControlledAnimator.SetTrigger("Reload");
            yield return new WaitForSeconds(clip.Length);
            ControlledAnimator.LastTrigger = "";
            Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon].Reload();
            ALLOW_FIRE_FLAG_3 = true;
        }
        public override void VerticalRotation(float Angle)
        {
            VR = Angle;
        }
        public int CurrentStandingMaterial = 0;
        float VolumeMultiplier = 1;
        public void PlayFootstep()
        {
            if (Physics.Raycast(CC.transform.position, Vector3.down, out RaycastHit hit, 5, GameRuntime.CurrentGlobals.LayerExcludePlayerAndAirBlock, QueryTriggerInteraction.Collide))
            {
                var SM = hit.collider.GetComponent<StandMaterial>();
                if (SM != null)
                {
                    CurrentStandingMaterial = SM.MaterialID;
                    VolumeMultiplier = SoundResources.FindMultiplier(CurrentStandingMaterial);
                }
            }
            if (FootStepSoundSource != null)
            {
                FootStepSoundSource.Stop();
                FootStepSoundSource.volume = VolumeMultiplier * CurrentFootStepMultiplier;
                FootStepSoundSource.clip = SoundResources.ObtainOneClip(CurrentStandingMaterial);
                FootStepSoundSource.Play();
            }
            if (Entity.isTookControl)
            {
                if (SpeedMultiplyer == SprintMultiplyer)
                    ABC.ShakeCamRunStep();
                if (SpeedMultiplyer == 1)
                    ABC.ShakeCamWalkStep();
            }
        }
        public override void Melee()
        {
            if (SpeedMultiplyer == SprintMultiplyer)
                CancelRun();
            CancelZoom();
            if (!ALLOW_FIRE_FLAG_1) return;
            if (!ALLOW_FIRE_FLAG_2) return;
            var Clip = this.ControlledAnimator.SetTrigger("Melee");
            StartCoroutine(MeleeProcess(Clip));
        }
        public IEnumerator MeleeProcess(Site13AnimationClip clip)
        {
            ALLOW_FIRE_FLAG_2 = false;
            yield return new WaitForSeconds(clip.Length);
            ControlledAnimator.LastTrigger = "";
            ALLOW_FIRE_FLAG_2 = true;
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
        public override void Zoom()
        {
            if (SpeedMultiplyer == SprintMultiplyer)
                CancelRun();
            if (!ALLOW_FIRE_FLAG_1) return;
            if (!ALLOW_FIRE_FLAG_2) return;
            if (!ALLOW_FIRE_FLAG_0) return;
            if (!ALLOW_FIRE_FLAG_3) return;
            if (Entity.EntityBag.Weapons.Count > 0)
            {
                Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon].AimingMode = 1;
                MainCamera.Instance.TargetScale = Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon].ZoomScale;
            }
            if (IsFPSBiped)
            {
                if (WeaponLayerCamera.Instance.RealCamera.enabled == true)
                    WeaponLayerCamera.Instance.RealCamera.enabled = false;
                return;
            }
            ControlledAnimator.SetTrigger("Zoom");
        }
        public override void CancelZoom()
        {
            if (Entity.EntityBag.Weapons.Count > 0)
            {
                Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon].AimingMode = 0;
                MainCamera.Instance.TargetScale = 1;
            }
            if (IsFPSBiped)
            {
                if (WeaponLayerCamera.Instance.RealCamera.enabled == false)
                    WeaponLayerCamera.Instance.RealCamera.enabled = true;
                return;
            }
            ControlledAnimator.SetTrigger("CancelZoom");
        }
        public override void SwitchWeapon()
        {
            CancelRun();
            if (Entity.EntityBag.Weapons.Count == 1)
                return;
            if (Entity.EntityBag.Weapons.Count > 1)
                Entity.EntityBag.CurrentWeapon = (Entity.EntityBag.CurrentWeapon == 1 ? 0 : 1);
            Entity.OnSwapWeapon.Invoke();
            CancelZoom();
            StartCoroutine(__TakeoutAnimation());
        }
        IEnumerator __TakeoutAnimation()
        {
            ALLOW_FIRE_FLAG_1 = false;
            yield return null;
            var clip = ControlledAnimator.SetTrigger("Takeout");
            yield return new WaitForSeconds(clip.Length);
            ControlledAnimator.LastTrigger = "";
            ALLOW_FIRE_FLAG_1 = true;

        }
        public override void Run()
        {
            //ControlledAnimator.SetTrigger("FSMR-SM2");
            //ControlledAnimator.SetTrigger("Run");
            SpeedMultiplyer = SprintMultiplyer;
            ALLOW_FIRE_FLAG_0 = false;
        }
        public override void CancelRun()
        {
            //ControlledAnimator.SetTrigger("Idle");
            ALLOW_FIRE_FLAG_0 = true;
            if (SpeedMultiplyer == SprintMultiplyer)
                SpeedMultiplyer = 1;
        }
        public override void Crouch()
        {
            SpeedMultiplyer = CrouchMultiplyer;
        }
        public override void CancelCrouch()
        {
            SpeedMultiplyer = 1;
        }
        public bool Fire = false;
        public override void StartFire()
        {
            CancelRun();
            if (!ALLOW_FIRE_FLAG_0) return;
            if (!ALLOW_FIRE_FLAG_1) return;
            if (!ALLOW_FIRE_FLAG_2) return;
            if (!ALLOW_FIRE_FLAG_3) return;
            if (!Fire)
            {
                if (Entity.EntityBag.Weapons.Count >= Entity.EntityBag.CurrentWeapon + 1)
                    if (Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon].WeaponData.CurrentMagazine > 0)
                        Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon].Fire();
                Fire = true;
            }
        }
        public override void CancelFire()
        {
            if (Fire)
            {
                if (Entity.EntityBag.Weapons.Count >= Entity.EntityBag.CurrentWeapon + 1)
                    Entity.EntityBag.Weapons[Entity.EntityBag.CurrentWeapon].Unfire();
                Fire = false;
            }
        }
        public override void Interact()
        {
            if (ALLOW_FIRE_FLAG_1)
                Interactor.Interact();
        }
        public override void CancelInteract()
        {
            Interactor.UnInteract();
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
        public float DropTime = 0;
        public float ActiveMovementTime;
        public float WalkInteval;
        public float RunInteval;
        public float CrouchInteval;
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
                        CurrentFootStepMultiplier = HeavyLandFootStepMultiplier;
                        PlayFootstep();
                        ControlledAnimator.SetTrigger("HeavyLand");
                    }
                }
                else if (DropTime >= 0.1f)
                {
                    CurrentFootStepMultiplier = LandFootStepMultiplier;
                    PlayFootstep();
                }
                DropTime = 0;
            }
            else
            {
                if (DropTime >= 0.1f)
                {
                    ControlledAnimator.SetTrigger("Idle");
                }
                DropTime += DT;
            }
            CC.Move(_MOVE * DT);

            if ((new Vector2(CC.velocity.x, CC.velocity.z)).sqrMagnitude > 0.1f)
            {
                if (CC.isGrounded)
                    if (MH != 0 || MV != 0)
                    {
                        ActiveMovementTime += DT;

                        if (SpeedMultiplyer == SprintMultiplyer)
                        {
                            if (ActiveMovementTime >= RunInteval)
                            {
                                CurrentFootStepMultiplier = SprintFootStepMultiplier;
                                ActiveMovementTime = 0;
                                PlayFootstep();
                            }
                        }
                        else
                                    if (SpeedMultiplyer == 1)
                        {
                            if (ActiveMovementTime >= WalkInteval)
                            {
                                CurrentFootStepMultiplier = WalkFootStepMultiplier;
                                ActiveMovementTime = 0;
                                PlayFootstep();
                            }
                        }
                        else
                        {
                            if (ActiveMovementTime >= CrouchInteval)
                            {
                                CurrentFootStepMultiplier = CrouchFootStepMultiplier;
                                ActiveMovementTime = 0;
                                PlayFootstep();
                            }
                        }
                        switch (ControlledAnimator.LastTrigger)
                        {
                            case "Run":
                            case "Walk":
                            case "Idle":
                            case "Crouch":
                            case "HeavyLand":
                            case "Pickup":
                            case "":
                                {
                                    if (SpeedMultiplyer == SprintMultiplyer)
                                    {
                                        //Running.
                                        ControlledAnimator.SetTrigger("Run");
                                    }
                                    else
                                    if (SpeedMultiplyer == 1)
                                    {
                                        //Running.
                                        ControlledAnimator.SetTrigger("Walk");
                                    }
                                    else
                                    {
                                        ControlledAnimator.SetTrigger("Crouch");
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
            }
            else
            {
                switch (ControlledAnimator.LastTrigger)
                {
                    case "Run":
                    case "Crouch":
                    case "Walk":
                        ControlledAnimator.SetTrigger("Idle");
                        break;
                    default:
                        break;
                }
            }
        }
        void _Crouch(float DT)
        {
            if (SpeedMultiplyer == CrouchMultiplyer)
            {
                if (CC.height > CrouchCharacterHeight)
                {
                    if (CC.skinWidth != CrouchSkinWidth)
                        CC.skinWidth = CrouchSkinWidth;

                    float delta = (CrouchCharacterHeight - CC.height);
                    if (Mathf.Abs(delta) <= 0.02f)
                    {
                        CC.height = CrouchCharacterHeight;
                    }
                    else
                        CC.height += delta * CharacterHeightChangeSpeed * DT;
                }
                else
                {
                    if (CC.skinWidth != NormalSkinWidth)
                        CC.skinWidth = NormalSkinWidth;

                }
                if (CC.center.y != CrouchCharacterOffset_Y)
                {
                    var c = CC.center;
                    c.y += (CrouchCharacterOffset_Y - c.y) * CharacterHeightChangeSpeed * DT;
                    CC.center = c;
                }
            }
            else
            {
                if (CC.height < NormalCharacterHeight)
                {
                    if (CC.skinWidth != CrouchSkinWidth)
                        CC.skinWidth = CrouchSkinWidth;
                    float delta = (NormalCharacterHeight - CC.height);
                    if (Mathf.Abs(delta) <= 0.02f)
                    {
                        CC.height = NormalCharacterHeight;
                    }
                    else
                        CC.height += delta * CharacterHeightChangeSpeed * DT;
                }
                else
                {
                    if (CC.skinWidth != NormalSkinWidth)
                        CC.skinWidth = NormalSkinWidth;

                }
                if (CC.center.y != NormalCharacterOffset_Y)
                {
                    var c = CC.center;
                    c.y += (NormalCharacterOffset_Y - c.y) * CharacterHeightChangeSpeed * DT;
                    CC.center = c;
                }
            }
        }
        public void OnFrame(float DT, float UDT)
        {
            {
                _Crouch(DT);
                Rotation(DT);
                Move(DT);
            }
            {
                ControlledAnimator.AccumulativeTrigger(DT);
            }
        }
    }
}
