using Site13Kernel.Core;
using Site13Kernel.Diagnostics.Functions;
using Site13Kernel.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Site13Kernel.Diagnostics
{
    public class DebuggerUI : ControlledBehavior
    {
        public Text Output;
        public InputField _Input;
        public TextBox __Input;
        public override void Init()
        {
            Parent.RegisterRefresh(this);
            Parent.RegisterFixedRefresh(this);

            if (Output != null)
            {
                Output.text += "<b>SITE-13 CONSOLE</b>";
                Debugger.CurrentDebugger.Register((string content, LogLevel level) =>
                {
                    Output.text += "\n";
                    switch (level)
                    {
                        case LogLevel.Normal:
                            Output.text += content;
                            break;
                        case LogLevel.Warning:
                            Output.text += "<color=\"#FFAA00\">";
                            Output.text += content;
                            Output.text += "</color>";
                            break;
                        case LogLevel.Error:
                            Output.text += "<color=\"#FF0000\">";
                            Output.text += content;
                            Output.text += "</color>";
                            break;
                        default:
                            break;
                    }
                });
                Debugger.CurrentDebugger.Register(() =>
                {
                    Output.text = "<b>SITE-13 CONSOLE</b>";
                });
            }
            FunctionCollection.GatherFunctions();
            //#if DEBUG

            Debugger.CurrentDebugger.Register((a, b) =>
            {
                StackTrace st = new StackTrace();
                var CALLER = st.GetFrame(4).GetMethod().DeclaringType;
                switch (b)
                {
                    case LogLevel.Normal:
                        UnityEngine.Debug.Log(a);
                        break;
                    case LogLevel.Warning:
                        UnityEngine.Debug.LogWarning(a);
                        break;
                    case LogLevel.Error:
                        UnityEngine.Debug.LogError($"{CALLER}:{a}");
                        break;
                    default:
                        break;
                }
            });
            //#endif
            if (_Input != null)
            {

                _Input.onSubmit.AddListener((string cmd) =>
                {
                    if (GameRuntime.CurrentGlobals.isDebugFunctionEnabled)
                        if (cmd != "")
                        {
                            ScriptEngine.Execute(cmd);
                        }
                        else
                        {
                            Debug.Log("You entered en empty string, is it by design?");
                        }
                    _Input.text = "";
                });
            }
            if (__Input != null)
            {

                __Input.onSubmit.AddListener((string cmd) =>
                {
                    if (GameRuntime.CurrentGlobals.isDebugFunctionEnabled)
                        if (cmd != "")
                        {
                            ScriptEngine.Execute(cmd);
                        }
                        else
                        {
                            Debug.Log("You entered en empty string, is it by design?");
                        }
                    __Input.text = "";
                });
            }
        }
        CursorLockMode LastLock;
        bool LastVisible;
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (this.gameObject.activeSelf)
                {
                    this.gameObject.SetActive(false);
                    Cursor.lockState = LastLock;
                    Cursor.visible = LastVisible;
                    GameRuntime.CurrentGlobals.isPaused = false;
                }
                else
                {
                    this.gameObject.SetActive(true);
                    GameRuntime.CurrentGlobals.isPaused = true;
                    LastLock = Cursor.lockState;
                    Cursor.lockState = CursorLockMode.None;
                    LastVisible = Cursor.visible;
                    Cursor.visible = true;
                }
            }
        }
        public override void FixedRefresh(float DeltaTime, float UnscaledDeltaTime)
        {
            Debugger.CurrentDebugger.LSync();
        }
    }
}
