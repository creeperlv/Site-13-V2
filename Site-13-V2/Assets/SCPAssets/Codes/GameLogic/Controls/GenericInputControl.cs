using Site13Kernel.Core;
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
            if (UseInputProcessor)
            {

            }
        }
    }
}
