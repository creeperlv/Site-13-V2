using Site13Kernel.Core;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.Character;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.Controls
{
    public class GenericInputControl : ControlledBehavior
    {
        public static GenericInputControl Instance;
        public bool isUIControl;
        public bool UseInputProcessor;
        public bool ControlledBehaviorWorkflow;
        float AccumulativeInvokeTime = 0;
        public float InvokeMinTime = 0.2f;
        bool ZOOM_FLG_0 = false;
        bool GRENAGE_FLAG = false;
        public void Start()
        {
            Instance = this;
        }
        public override void Init()
        {
            if (ControlledBehaviorWorkflow)
            {
                this.Parent.RegisterRefresh(this);
            }
        }
        public void Update()
        {
            if (ControlledBehaviorWorkflow)
            {
                return;
            }
            float dt = Time.deltaTime;
            float udt = Time.unscaledDeltaTime;
            AtFrame(dt, udt);
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            AtFrame(DeltaTime, UnscaledDeltaTime);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AtFrame(float deltaTime, float unscaledDeltaTime)
        {
            if (isUIControl)
            {
                UIControl(deltaTime, unscaledDeltaTime);
            }
            else
            {
                GenericControllerControl(deltaTime, unscaledDeltaTime);
            }
        }
        public void UIControl(float deltaTime, float unscaledDeltaTime)
        {

        }
        public void GenericControllerControl(float deltaTime, float unscaledDeltaTime)
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            var controller = TakeControl.Instance.controller;
            if (controller == null) return;
            if (UseInputProcessor)
            {
                if (controller.ControllerFunctions.Movement)
                {
                    var H = InputProcessor.GetAxis("MoveHorizontal");
                    var V = InputProcessor.GetAxis("MoveVertical");
                    controller.Move(new Vector2(V, H), deltaTime);
                }
                if (controller.ControllerFunctions.Sprint)
                {
                    if (InputProcessor.GetInputDown("Run"))
                    {
                        controller.Run();
                    }
                    else if (InputProcessor.GetInputUp("Run"))
                    {
                        controller.CancelRun();
                    }
                }
                if (controller.ControllerFunctions.Jump)
                {
                    if (InputProcessor.GetInputDown("Jump"))
                    {
                        controller.Jump();
                    }
                }
                if (controller.ControllerFunctions.Crouch)
                {
                    if (InputProcessor.GetInputDown("Crouch"))
                    {
                        controller.Crouch();
                    }
                    else if (InputProcessor.GetInputUp("Crouch"))
                    {
                        controller.CancelCrouch();
                    }
                }
                if (controller.ControllerFunctions.Interact)
                {

                    if (InputProcessor.GetInput("Interact"))
                    {
                        AccumulativeInvokeTime += deltaTime;
                        if (AccumulativeInvokeTime > InvokeMinTime)
                        {
                            AccumulativeInvokeTime = 0;
                            controller.Interact();
                        }
                    }
                    else
                    {
                        AccumulativeInvokeTime = 0;
                        controller.CancelInteract();
                    }
                }
                if (controller.ControllerFunctions.Fire)
                {
                    if (InputProcessor.GetAxis("Fire") > 0.5f)
                    {
                        controller.StartFire();
                    }
                    else
                        controller.CancelFire();
                }
                if (controller.ControllerFunctions.Melee)
                {

                    if (InputProcessor.GetInputDown("Combat"))
                    {
                        controller.Melee();
                    }
                }
                if (controller.ControllerFunctions.SwitchWeapon)
                {

                    if (InputProcessor.GetInputDown("SwitchWeapon"))
                    {
                        controller.SwitchWeapon();
                    }
                }
                if (controller.ControllerFunctions.Reload)
                {

                    if (InputProcessor.GetInputDown("Reload"))
                    {
                        controller.Reload();
                    }
                }
                if (controller.ControllerFunctions.Zoom)
                {

                    if (InputProcessor.GetInputDown("Zoom"))
                    {
                        if (ZOOM_FLG_0 == false)
                        {
                            controller.Zoom();
                            ZOOM_FLG_0 = true;
                        }
                    }
                    else if (InputProcessor.GetInputUp("Zoom"))
                    {
                        if (ZOOM_FLG_0 == true)
                        {
                            controller.CancelZoom();
                            ZOOM_FLG_0 = false;
                        }
                    }
                }
                if (controller.ControllerFunctions.Grenade)
                {
                    if (InputProcessor.GetAxis("ThrowGrenade") > 0.4f)
                    {
                        if (GRENAGE_FLAG == false)
                        {
                            GRENAGE_FLAG = true;
                        controller.ThrowGrenade();
                        }
                    }
                    else
                    {
                        if (GRENAGE_FLAG)
                        {
                            GRENAGE_FLAG = false;
                        }
                    }
                }
                if (controller.ControllerFunctions.SwitchGrenade)
                {
                    if (InputProcessor.GetInputDown("SwitchGrenade"))
                    {
                        controller.SwitchGrenade();
                    }
                }
                if (controller.ControllerFunctions.ViewportRotation)
                {
                    var H = InputProcessor.GetAxis("MouseH");
                    var V = InputProcessor.GetAxis("MouseV");
                    controller.HorizontalRotation(H);
                    controller.VerticalRotation(V);
                }
            }
        }
    }
}
