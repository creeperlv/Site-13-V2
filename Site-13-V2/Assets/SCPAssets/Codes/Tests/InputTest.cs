using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Tests
{
    public class InputTest : ControlledBehavior
    {
        public override void Refresh(float DeltaTime)
        {
            StringBuilder builder=new StringBuilder();
            builder.Append($"Btn0:{Input.GetKeyDown(KeyCode.Joystick1Button0)},");
            builder.Append($"Btn1:{Input.GetKeyDown(KeyCode.Joystick1Button1)},");
            builder.Append($"Btn2:{Input.GetKeyDown(KeyCode.Joystick1Button2)},");
            builder.Append($"Btn3:{Input.GetKeyDown(KeyCode.Joystick1Button3)},");
            Debug.Log(builder.ToString());
        }
    }
}
