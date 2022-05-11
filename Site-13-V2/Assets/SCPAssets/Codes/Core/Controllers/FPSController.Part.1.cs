using CLUNL.Localization;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.Core.Interactives;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using Site13Kernel.GameLogic.FPS;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.Core.Controllers
{
    public partial class FPSController
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (Interrupt00) return;
            if (Time.timeScale == 0) return;//The game is absolutely paused. :P
            if (GameRuntime.CurrentGlobals.isPaused)
            {
                return;
            }
            if (FrameDelay > 0)
            {
                FrameDelay--;
                return;
            }
            if (BagHolder.VerifyWeaponSlot())
            {
                ApplySwitchWeapon();
            }
            WeaponControl(DeltaTime, UnscaledDeltaTime);
            Weapon = BagHolder.CurrentWeapon == 0 ? BagHolder.Weapon0 : BagHolder.Weapon1;
            if(MainCam!=null)
            Zoom(DeltaTime);
            Movement(DeltaTime, UnscaledDeltaTime);
            if (Weapon != null)
            {
                FireControl(DeltaTime);
            }
            Interact(DeltaTime, UnscaledDeltaTime);
            {
                //Weapons
                if (Weapon != null)
                    Weapon.Refresh(DeltaTime, UnscaledDeltaTime);
            }
            if (FlashLightEnabled)
                FlashLight();
            Grenade(DeltaTime, UnscaledDeltaTime);
            BodyAnimation(DeltaTime, UnscaledDeltaTime);
            UpdateHUD(DeltaTime, UnscaledDeltaTime);
            SRBoCC.Refresh(DeltaTime, UnscaledDeltaTime);
        }
        bool Grenade0 = false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Grenade(float DeltaTime, float UnscaledDeltaTime)
        {
            if (InputProcessor.GetAxis("ThrowGrenade") > 0.4f)
            {
                if (Grenade0 == false)
                {
                    if (Grenade_Throwing == false)
                    {
                        if (Weapon != null)
                        {
                            if (Weapon.Weapon.WeaponMode != WeaponConstants.WEAPON_MODE_NORMAL)
                            {
                                return;
                            }
                        }
                        ProcessedGrenade PG = BagHolder.CurrentGrenade == 0 ? BagHolder.Grenade0 : BagHolder.Grenade1;
                        if (PG.GrenadeHashCode != -1)
                        {
                            if (PG.RemainingCount > 0)
                            {
                                if (Weapon != null)
                                {
                                    Weapon.gameObject.SetActive(false);
                                    Weapon.Weapon.ResetTakeOut();
                                }
                                CancelRun();
                                toZoom = false;
                                GrenadeThrowD = 0;
                                Grenade_Throwing = true;
                                Grenade_Throwed = false;
                                GrenadeThrower.gameObject.SetActive(true);
                            }
                        }
                        //GrenadeThrower.playbackTime=0;
                    }
                    Grenade0 = true;
                }
            }
            else Grenade0 = false;
            if (InputProcessor.GetInputDown("SwitchGrenade"))
            {
                BagHolder.CurrentGrenade = (BagHolder.CurrentGrenade == 0 ? 1 : 0);
            }
            if (Grenade_Throwing)
            {

                GrenadeThrowD += DeltaTime;
                if (GrenadeThrowD > GrenadeThrowTime)
                {
                    if (Grenade_Throwed == false)
                    {
                        ProcessedGrenade PG = BagHolder.CurrentGrenade == 0 ? BagHolder.Grenade0 : BagHolder.Grenade1;
                        if (PG.GrenadeHashCode != -1)
                        {
                            if (PG.RemainingCount > 0)
                            {
                                GrenadeController.CurrentController.Instantiate(
                                    GrenadePool.CurrentPool.GrenadeItemMap[PG.GrenadeHashCode].GamePlayPrefab,
                                    Grenade_ThrowOutPoint.position,
                                    Grenade_ThrowOutPoint.rotation,
                                    Grenade_ThrowOutPoint.forward * GrenadeThrowForce, ForceMode.Impulse);
                                Grenade_Throwed = true;

                                PG.RemainingCount--;
                            }
                        }
                    }
                }
                if (GrenadeThrowD > GrenadeThrowAnimationTime)
                {
                    Grenade_Throwing = false;
                    GrenadeThrower.gameObject.SetActive(false);
                    if (Weapon != null)
                        Weapon.gameObject.SetActive(true);
                }
            }
        }
        int FRAME_IGNORACED = 0;
        float ANIMATION_DELTA_T;
        Vector3 POSITION_0 = Vector3.zero;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BodyAnimation(float DeltaTime, float UnscaledDeltaTime)
        {
            if (AnimatedBodyEnabled && BodyAnimator != null)
            {

                if (FRAME_IGNORACED >= FRAMEIGNORANCE)
                {
                    FRAME_IGNORACED = 0;
                    //... Apply Animation Here.
                    if (POSITION_0 == Vector3.zero)
                    {
                        //Init.
                        POSITION_0 = this.transform.position;
                    }
                    else
                    {
                        var __D = (this.transform.position - POSITION_0).magnitude;
                        BodyAnimator.speed = Mathf.Min(__D / ANIMATION_DELTA_T * Intensity, MaxFinalIntensity);
                    }
                    ANIMATION_DELTA_T = 0;
                }
                ANIMATION_DELTA_T += DeltaTime;
                FRAME_IGNORACED++;
            }
        }

        float InteractTime;
        InteractiveBase Interactive = null;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SwapInteractive(InteractiveBase _Interactive)
        {
            if (_Interactive == null)
                if (Interactive != null)
                {
                    if (Interactive.isCollision)
                    {
                        return;
                    }
                }
            if (Interactive != _Interactive)
            {
                if (Interactive != null)
                {
                    UnInvoke(Interactive);
                }
            }
            Interactive = _Interactive;
            if (Interactive != null)
            {
                if (InteractHint != null)
                {
                    InteractHint.Visibility = true;
                    InteractHint.Content = Language.Find(Interactive.OperateHint, Interactive.OperateHintFallBack);
                }
            }
            else
            {
                if (InteractHint != null)
                {
                    InteractHint.Visibility = false;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Interact(float DeltaTime, float UnscaledDeltaTime)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0F));
            {
                // On Raycast Detection
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Reach, GameRuntime.CurrentGlobals.LayerExcludePlayerAndAirBlock, QueryTriggerInteraction.Collide))
                {
                    GameObject TARGET_OBJ = hit.transform.gameObject;
                    var _Interactive = TARGET_OBJ.GetComponent<InteractiveBase>();
                    if (_Interactive != null)
                    {
                        if (_Interactive.InteractiveMode == InteractiveMode.OnAim || _Interactive.isCollision)
                        {
                            SwapInteractive(_Interactive);
                        }
                    }
                    else
                    {
                        SwapInteractive(_Interactive);
                    }
                    if (_Interactive != null)
                        if (Interactive != null)
                        {

                            if (Interactive.InvokeMode == InvokeMode.ACTIVE)
                            {
                                IInvoke(Interactive, DeltaTime, UnscaledDeltaTime);

                            }

                        }
                }
                else if (Physics.Raycast(ray, out hit, SightDistance, GameRuntime.CurrentGlobals.LayerExcludePlayerAndAirBlock, QueryTriggerInteraction.Collide))
                {
                    GameObject TARGET_OBJ = hit.transform.gameObject;
                    var _Interactive = TARGET_OBJ.GetComponent<InteractiveBase>();
                    if (_Interactive != null)
                    {
                        if (_Interactive.DistanceMode == DistanceMode.OnSight)
                            if (_Interactive.InteractiveMode == InteractiveMode.OnAim || _Interactive.isCollision)
                            {
                                SwapInteractive(_Interactive);
                            }
                    }
                    else
                    {
                        SwapInteractive(_Interactive);
                    }
                    if (_Interactive != null)
                        if (Interactive != null)
                        {

                            if (Interactive.InvokeMode == InvokeMode.ACTIVE)
                            {
                                IInvoke(Interactive, DeltaTime, UnscaledDeltaTime);

                            }

                        }
                }
                else
                {
                    SwapInteractive(null);

                }
                if (Interactive != null)
                {
                    if (Interactive.InvokeMode == InvokeMode.PASSIVE)
                        if (InputProcessor.GetInput("Interact"))
                        {
                            if (InteractTime < InteractSensitivity)
                                InteractTime += UnscaledDeltaTime;
                            if (InteractTime > InteractSensitivity)
                            {
                                IInvoke(Interactive, DeltaTime, UnscaledDeltaTime);
                                InteractTime = 0;
                            }
                        }
                    if (InputProcessor.GetInputUp("Interact"))
                    {
                        UnInvoke(Interactive);
                    }
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IInvoke(InteractiveBase Interactive, float DeltaTime, float UnscaledDeltaTime)
        {
            if (Interactive.OperationMode == OperationMode.SingleFrame)
            {
                if (Interactive.isOperating != true)
                {
                    Interactive.Operate(DeltaTime, UnscaledDeltaTime, CurrentEntity);
                    Interactive.isOperating = true;
                }
            }
            else
            {

                Interactive.Operate(DeltaTime, UnscaledDeltaTime, CurrentEntity);
                if (Interactive.isOperating != true)
                {
                    Interactive.isOperating = true;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnInvoke(InteractiveBase Interactive)
        {
            if (Interactive.isOperating)
                Interactive.UnOperate();
            Interactive.isOperating = false;

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetState(MoveState State)
        {
            switch (State)
            {
                case MoveState.Walk:
                    FootStepSoundSource.volume = NormalFootStepVolume * VolumeMultiplier;
                    break;
                case MoveState.Run:
                    FootStepSoundSource.volume = RunningFootStepVolume * VolumeMultiplier;
                    break;
                case MoveState.Crouch:
                    FootStepSoundSource.volume = CrouchFootStepVolume * VolumeMultiplier;
                    break;
                default:
                    break;
            }
            MovingState = State;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(float DeltaTime)
        {
            if (InputProcessor.GetInputDown("Run") && toZoom == false)
            {
                if (Weapon != null)
                {
                    if (Weapon.Weapon.WeaponMode != WeaponConstants.WEAPON_MODE_NORMAL)
                    {
                        return;
                    }
                }
                if (MovingState == MoveState.Walk)
                {
                    if (Weapon != null)
                    {

                        if (Weapon.Weapon.WeaponMode == WeaponConstants.WEAPON_MODE_RELOAD_STAGE_1 || Weapon.Weapon.WeaponMode == WeaponConstants.WEAPON_MODE_RELOAD_STAGE_1)
                        {

                        }
                        else
                            SetState(MoveState.Run);
                    }
                    else
                        SetState(MoveState.Run);
                }
            }
            if (InputProcessor.GetInputUp("Run"))
            {
                if (MovingState == MoveState.Run)
                {
                    SetState(MoveState.Walk);
                }
                //isRunning = false;
            }
            if (MovingState == MoveState.Run)
            {
                if (WRTween < 1)
                {
                    isWalking = false;
                    WRTween += DeltaTime * FPSCamSwingIntensitySwitchSpeed;
                    //ApplyWR(DeltaTime);
                }
                else
                {
                    if (WRTween != 1)
                    {
                        WRTween = 1;
                        //ApplyWR(DeltaTime);
                    }
                }
            }
            else
            {
                if (WRTween > 0)
                {
                    WRTween -= DeltaTime * FPSCamSwingIntensitySwitchSpeed;
                    //ApplyWR(DeltaTime);
                }
                else
                {
                    if (WRTween != 0)
                    {
                        WRTween = 0;
                        isWalking = true;
                    }
                }

            }
            ApplyWR(DeltaTime);
        }
        private void OnTriggerEnter(Collider other)
        {
            var interactive = other.gameObject.GetComponent<InteractiveBase>();
            if (interactive != null)
            {
                interactive.isCollision = true;
                SwapInteractive(interactive);
                if (interactive is Pickupable p)
                {
                    p.ObtainRemaining(BagHolder);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            var interactive = other.gameObject.GetComponent<InteractiveBase>();
            if (interactive != null)
            {
                if (interactive.isCollision && interactive == Interactive)
                {
                    Interactive.isCollision = false;
                    UnInvoke(Interactive);
                    SwapInteractive(null);
                }
            }
        }
    }
}
