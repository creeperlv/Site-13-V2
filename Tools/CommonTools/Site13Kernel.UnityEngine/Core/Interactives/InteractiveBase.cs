using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Site13Kernel.Core.Interactives
{
    public class InteractiveBase : ControlledBehavior
    {
        public InteractiveMode InteractiveMode;
        public OperationMode OperationMode;
        public InvokeMode InvokeMode;
        public DistanceMode DistanceMode;
        public bool isOperating=false;
        public bool isCollision=false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Operate(float DeltaTime,float UnscaledDeltaTime,DamagableEntity Operator)
        {
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void UnOperate()
        {
        }
        public string OperateHint;
        public string OperateHintFallBack;
    }
}
