using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Site13Kernel.GameLogic
{
    public class CampaignDirector : ControlledBehavior
    {
        SerialCampaignScript script;
        public static CampaignDirector CurrentDirector;
        public override void Init()
        {
            CurrentDirector = this;
            var SCRIPT = GameRuntime.CurrentGlobals.Scripts[GameRuntime.CurrentGlobals.CurrentMission.TargetScript];
            StartCoroutine(ExecuteScript(SCRIPT.text));
            //script = GameRuntime.CurrentLocals.CurrentScipt;
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool isComment(string Line)
        {
            var L = Line = Line.Trim();
            string Prefix;
            switch (L[0])
            {
                case '#':
                case ';':
                case '!':
                case '?':
                case '<':
                case '[':
                case '-':
                case '*':
                    return true;
            }
            if (L.Length > 2)
            {
                Prefix = L.Substring(0, 2);
                switch (Prefix)
                {
                    case "//":
                    case "/*":
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }
        public IEnumerator ExecuteScript(string Content)
        {
            StringReader TR = new StringReader(Content);
            string Line;
            while ((Line = TR.ReadLine()) != null)
            {

                var line = Line.Trim();
                if (isComment(Line))
                {

                }
                else
                {
                    if (line.ToUpper() == "NOP")
                    {
                        yield return null;
                    }
                    else if (line.StartsWith("WAIT "))
                    {
                        var T = line.Substring(4).Trim();
                        if (float.TryParse(T, out float t))
                        {
                            yield return new WaitForSeconds(t);
                        }
                    }
                    else
                    {
                        ScriptEngine.Execute(line);
                    }
                }
            }
        }
        public void Win()
        {
            GlobalBioController.CurrentGlobalBioController.DestoryAll();
            SceneLoader.Instance.SetStick(GameRuntime.CurrentGlobals.Scene_LevelBase, false);
            SceneLoader.Instance.LoadScene(GameRuntime.CurrentGlobals.Scene_WinScene, true, false, false);
        }
        public override void FixedRefresh(float DeltaTime, float UnscaledDeltaTime)
        {
        }
    }
}
