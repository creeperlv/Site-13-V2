using CLUNL.Localization;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.Core.Interactives;
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
            if (FrameDelay > 0)
            {
                FrameDelay--;
                return;
            }
            Weapon = BagHolder.CurrentWeapon == 0 ? BagHolder.Weapon0 : BagHolder.Weapon1;
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
            BodyAnimation(DeltaTime, UnscaledDeltaTime);
            UpdateHUD();
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
                        if (InputProcessor.CurrentInput.GetInput("Interact"))
                        {
                            if (InteractTime < InteractSensitivity)
                                InteractTime += UnscaledDeltaTime;
                            if (InteractTime > InteractSensitivity)
                            {
                                IInvoke(Interactive, DeltaTime, UnscaledDeltaTime);
                            }
                        }
                    if (InputProcessor.CurrentInput.GetInputUp("Interact"))
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
                    FootStepSoundSource.volume = NormalFootStepVolume;
                    break;
                case MoveState.Run:
                    FootStepSoundSource.volume = RunningFootStepVolume;
                    break;
                case MoveState.Crouch:
                    FootStepSoundSource.volume = CrouchFootStepVolume;
                    break;
                default:
                    break;
            }
            MovingState = State;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(float DeltaTime)
        {
            if (InputProcessor.CurrentInput.GetInputDown("Run") && toZoom == false)
            {
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
            if (InputProcessor.CurrentInput.GetInputUp("Run"))
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
                    ApplyWR(DeltaTime);
                }
                else
                {
                    if (WRTween != 1)
                    {
                        WRTween = 1;
                        ApplyWR(DeltaTime);
                    }
                }
            }
            else
            {
                if (WRTween > 0)
                {
                    WRTween -= DeltaTime * FPSCamSwingIntensitySwitchSpeed;
                    ApplyWR(DeltaTime);
                }
                else
                {
                    if (WRTween != 0)
                    {
                        WRTween = 0;
                        ApplyWR(DeltaTime);
                        isWalking = true;
                    }
                }

            }
        }
        private void OnTriggerEnter(Collider other)
        {
            var interactive = other.gameObject.GetComponent<InteractiveBase>();
            if (interactive != null)
            {
                interactive.isCollision = true;
                SwapInteractive(interactive);
                if(interactive is Pickupable p)
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
                    Debugger.CurrentDebugger.Log("0xFF00FF0001");
                }
            }
        }
    }
}
