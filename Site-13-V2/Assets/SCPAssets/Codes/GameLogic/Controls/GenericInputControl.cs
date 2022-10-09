using Site13Kernel.Core;
using Site13Kernel.Core.CustomizedInput;
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
            if(Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState=CursorLockMode.Locked;
                Cursor.visible = false;
            }
            var controller = BasicController.Instance;
            if (controller == null) return;
            if (UseInputProcessor)
            {
                if (controller.ControllerFunctions.Movement)
                {
                    var H = InputProcessor.GetAxis("MoveHorizontal");
                    var V = InputProcessor.GetAxis("MoveVertical");
                    BasicController.Instance.Move(new Vector2(V, H), deltaTime);
                }
                if (controller.ControllerFunctions.Sprint)
                {
                    if (InputProcessor.GetInputDown("Run"))
                    {
                        BasicController.Instance.Run();
                    }else if (InputProcessor.GetInputUp("Run"))
                    {
                        BasicController.Instance.CancelRun();
                    }
                }
                if (controller.ControllerFunctions.Jump)
                {
                    if (InputProcessor.GetInputDown("Jump"))
                    {
                        BasicController.Instance.Jump();
                    }
                }
                if (controller.ControllerFunctions.Crouch)
                {
                    if (InputProcessor.GetInputDown("Crouch"))
                    {
                        BasicController.Instance.Crouch();
                    }
                    else if (InputProcessor.GetInputUp("Crouch"))
                    {
                        BasicController.Instance.CancelCrouch();
                    }
                }
                if (controller.ControllerFunctions.ViewportRotation)
                {
                    var H = InputProcessor.GetAxis("MouseH");
                    var V = InputProcessor.GetAxis("MouseV");
                    BasicController.Instance.HorizontalRotation(H);
                    BasicController.Instance.VerticalRotation(V);
                }
            }
        }
    }
}
