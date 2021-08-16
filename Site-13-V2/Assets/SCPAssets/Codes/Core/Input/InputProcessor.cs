using Site13Kernel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core.CustomizedInput
{
    /// <summary>
    /// Run in the `Do not destory on load`
    /// </summary>
    public class InputProcessor : ControlledBehavior
    {
        public List<InputDefinition> InputDefinitions;
        public override void Init()
        {

        }
        public bool GetInputDown(string Name)
        {
            return false;
        }
        public bool GetInputUp(string Name)
        {
            return false;
        }
        public bool GetInput(string Name)
        {
            return false;
        }
        public float GetAxis(string Name)
        {
            return 0;
        }
        public override void Refresh(float DeltaTime)
        {

        }
    }
    /// <summary>
    /// A - Button 0, B - Button 1, X - Button 2, Y - Button 3
    /// </summary>
    [Serializable]
    public class InputDefinition
    {
        public string Name;
        public bool isButton;
        public bool AcceptMouse;
        public List<KeyCode> KeyCodes;
        public List<string> GamepadInput;

    }
}
