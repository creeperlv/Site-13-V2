using Site13Kernel.Core.Controllers;
using Site13Kernel.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Directors
{
    public class HLScriptableDirector : ScriptableDirector
    {
        public override void SetupActions()
        {
            Actions.Add(typeof(WinEvent), (e) =>
            {
                CampaignDirector.CurrentDirector.Win();
            });
            Actions.Add(typeof(SpawnPlayerEvent), (e) =>
            {
                if (e is SpawnPlayerEvent SPE) {
                    var L=FromSerializableLocation(SPE.Location);
                    try
                    {

                        var PLAYER = GlobalBioController.CurrentGlobalBioController.Spawn(SPE.PlayerID, Vector3.zero, Vector3.zero);
                        PLAYER.transform.GetChild(1).position = L.Position;
                        PLAYER.transform.GetChild(1).rotation = L.Rotation;
                        var FPSC = PLAYER.GetComponentInChildren<FPSController>();

                        LevelController.RegisterRefresh(FPSC);
                        FPSC.Parent = LevelController;
                        FPSC.Init();
                    }
                    catch (Exception _e)
                    {
                        Debugger.CurrentDebugger.LogError(_e);
                    }
                }
            });
        }
    }
}
