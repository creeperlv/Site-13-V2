using CLUNL.Utilities;
using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.Diagnostics
{
    public class DebuggerUI : ControlledBehavior
    {
        public Text Output;
        public InputField _Input;
        public override void Init()
        {
            Parent.OnFixedRefresh.Add(this);
            Parent.OnRefresh.Add(this);
            Output.text += "SITE-13 CONSOLE";
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
#if DEBUG
            Debugger.CurrentDebugger.Register((a, b) =>
            {
                switch (b)
                {
                    case LogLevel.Normal:
                        Debug.Log(a);
                        break;
                    case LogLevel.Warning:
                        Debug.LogWarning(a);
                        break;
                    case LogLevel.Error:
                        Debug.LogError(a);
                        break;
                    default:
                        break;
                }
            });
#endif
            _Input.onSubmit.AddListener((string cmd) =>
            {
                if (cmd != "")
                {
                    var result=CommandLineTool.Analyze(cmd);
                    var a=result.RealParameter;
                    Debugger.CurrentDebugger.Log($">{a[0].EntireArgument}", LogLevel.Normal);
                    
                }
                _Input.text = "";
            });
        }
        public override void Refresh(float DeltaTime)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                this.gameObject.SetActive(!this.gameObject.activeSelf);
            }
        }
        public override void FixedRefresh(float DeltaTime)
        {
            Debugger.CurrentDebugger.LSync();
        }
    }
}
