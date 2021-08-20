using CLUNL.Utilities;
using Site13Kernel.Core;
using Site13Kernel.Diagnostics.Functinos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.Diagnostics
{
    public class DebuggerUI : ControlledBehavior
    {
        public Text Output;
        public InputField _Input;
        Dictionary<string, IDiagnosticsFunction> _func=new Dictionary<string, IDiagnosticsFunction>();
        public override void Init()
        {
            Parent.RegisterRefresh(this);
            Parent.RegisterFixedRefresh(this);

            if (Output != null)
            {
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
            }
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
            if (_Input != null)
            {

                {
                    //Init functions.
                    var DLLS=AppDomain.CurrentDomain.GetAssemblies();
                    foreach (var DLL in DLLS)
                    {
                        foreach (var t in DLL.GetTypes())
                        {
                            if (t.GetInterface("IDiagnosticsFunction") != null)
                            {
                                var func=(IDiagnosticsFunction) Activator.CreateInstance(t);
                                _func.Add(func.GetCommandName().ToUpper(), func);
                            }
                        }
                    }
                }
                _Input.onSubmit.AddListener((string cmd) =>
                {
                    if (cmd != "")
                    {
                        var result=CommandLineTool.Analyze(cmd);
                        var a=result.RealParameter;
                        Debugger.CurrentDebugger.Log($">{a[0].EntireArgument}", LogLevel.Normal);
                        var CMD=a[0].EntireArgument.ToUpper();
                        if (_func.ContainsKey(CMD))
                        {
                            var func=_func[CMD];
                            a.RemoveAt(0);
                            func.Execute(a);
                        }
                        else
                        {
                            Debugger.CurrentDebugger.Log($"\"{a[0].EntireArgument}\" not found!", LogLevel.Warning);
                            Debugger.CurrentDebugger.Log($"Available functinos:", LogLevel.Normal);
                            foreach (var item in _func.Keys)
                            {
                                Debugger.CurrentDebugger.Log($"{item}", LogLevel.Normal);

                            }
                        }
                    }
                    else
                    {
                    }
                    _Input.text = "";
                });
            }
        }
        public override void Refresh(float DeltaTime)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (this.gameObject.activeSelf)
                {
                    this.gameObject.SetActive(false);

                }
                else
                {
                    this.gameObject.SetActive(true);
                
                }
            }
        }
        public override void FixedRefresh(float DeltaTime)
        {
            Debugger.CurrentDebugger.LSync();
        }
    }
}
