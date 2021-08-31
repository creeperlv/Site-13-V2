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
        public static InputProcessor CurrentInput;
        public Dictionary<string, float> Axis = new Dictionary<string, float>();
        public Dictionary<string, bool> KeyDown = new Dictionary<string, bool>();
        public Dictionary<string, bool> Key = new Dictionary<string, bool>();
        public Dictionary<string, bool> KeyUp = new Dictionary<string, bool>();
        public List<InputDefinition> InputDefinitions;
        public List<string> Names;
        public override void Init()
        {
            if (CurrentInput != null)
            {
                Parent.UnregisterRefresh(this);
                Parent.UnregisterFixedRefresh(this);
                //Parent.OnInit.Remove(this);
                Destroy(this.gameObject);
                return;
            }
            CurrentInput = this;
            Parent.RegisterRefresh(this);
            foreach (var item in InputDefinitions)
            {
                if (!Names.Contains(item.Name))
                    Names.Add(item.Name);
                if (!Axis.ContainsKey(item.Name))
                    Axis.Add(item.Name, 0);
                if (!KeyDown.ContainsKey(item.Name))
                    KeyDown.Add(item.Name, false);
                if (!Key.ContainsKey(item.Name))
                    Key.Add(item.Name, false);
                if (!KeyUp.ContainsKey(item.Name))
                    KeyUp.Add(item.Name, false);
            }
        }
        /// <summary>
        /// True on frame.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool GetInputDown(string Name)
        {
            return KeyDown[Name];
        }
        /// <summary>
        /// True on frame.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool GetInputUp(string Name)
        {
            return KeyUp[Name];
        }
        /// <summary>
        /// Trus during the press time.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool GetInput(string Name)
        {
            return Key[Name];
        }
        public float GetAxis(string Name)
        {
            return Axis[Name];

        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            //Debug.Log("Running");
            {

                for (int i = 0; i < Names.Count; i++)
                {
                    var item = Names[i];
                    Key[item] = false;
                    KeyDown[item] = false;
                    KeyUp[item] = false;
                    Axis[item] = 0;
                }
            }

            foreach (var item in InputDefinitions)
            {
                if (item.isButton)
                {
                    foreach (var key in item.PositiveKeys)
                    {
                        KeyDown[item.Name] |= Input.GetKeyDown(key);
                        Key[item.Name] |= Input.GetKey(key);
                        KeyUp[item.Name] |= Input.GetKeyUp(key);

                    }
                    foreach (var key in item.NegativeKeys)
                    {
                        KeyDown[item.Name] |= Input.GetKeyDown(key);
                        Key[item.Name] |= Input.GetKey(key);
                        KeyUp[item.Name] |= Input.GetKeyUp(key);
                    }
                }
                else
                {
                    foreach (var key in item.PositiveKeys)
                    {
                        var v = (Input.GetKey(key) ? 1 : 0) * item.Intensity;
                        if (v > Axis[item.Name])
                        {
                            Axis[item.Name] = v;
                        }
                    }
                    foreach (var key in item.NegativeKeys)
                    {
                        var v = (Input.GetKey(key) ? 1 : 0) * item.Intensity;
                        if (v > Mathf.Abs(Axis[item.Name]))
                        {

                            Axis[item.Name] = v;
                            Axis[item.Name] *= -1;
                        }
                    }
                    foreach (var key in item.GamepadInput)
                    {
                        var v = Input.GetAxis(key);
                        if (Mathf.Abs(v) > Mathf.Abs(Axis[item.Name]))
                        {
                            Axis[item.Name] = v;

                        }
                    }
                }

                if (item.AcceptMouse)
                {
                    float v = 0;
                    if (item.Axis == SnapAxis.X)
                    {
                        v = Input.GetAxis("Mouse Horizontal");

                    }
                    else if (item.Axis == SnapAxis.Y)
                    {
                        v = Input.GetAxis("Mouse Vertical");
                    }
                    if (Mathf.Abs(v) > item.DeadZone)
                    {
                        v *= item.Intensity;
                        if (Mathf.Abs(v) > Mathf.Abs(Axis[item.Name]))
                        {
                            Axis[item.Name] = v;
                        }
                    }

                }
            }
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
        public SnapAxis Axis;
        public float Intensity = 1;
        public float DeadZone = 0.12f;
        /// <summary>
        /// Produce Axis value as positive values.
        /// </summary>
        public List<KeyCode> PositiveKeys;
        /// <summary>
        /// Produce Axis value as negative values.
        /// </summary>
        public List<KeyCode> NegativeKeys;
        public List<string> GamepadInput;

    }
}
