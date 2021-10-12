using Site13Kernel.Core.Interactives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Tests
{
    public class InteractiveTest : InteractiveBase
    {
        public override void Operate(float DeltaTime, float UnscaledDeltaTime)
        {
            Debug.Log($"IntT>>OP: OP={OperationMode}, INT={InteractiveMode}, INV={InvokeMode}");
        }
        public override void UnOperate()
        {
            Debug.Log($"IntT>>UNOP: OP={OperationMode}, INT={InteractiveMode}, INV={InvokeMode}");
        }
    }
}
