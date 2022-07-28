using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
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
                if (e is SpawnPlayerEvent SPE)
                {
                    var L = FromSerializableLocation(SPE.Location);
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
            Actions.Add(typeof(ToggleObject), (e) =>
            {
                if (e is ToggleObject to)
                {
                    __ReferencedObjects[to.ObjectID].gameObject.SetActive(to.TargetState);
                }
            });
            Actions.Add(typeof(TogglePlayer), (e) =>
            {
                if (e is TogglePlayer to)
                {
                    FPSController.Instance.gameObject.SetActive(to.TargetState);
                }
            });
            Actions.Add(typeof(ShowSubtitle), (e) =>
            {
                if (e is ShowSubtitle ss)
                {
                    Subtitle subtitle = new Subtitle();
                    subtitle.Content = ss.Content;
                    subtitle.Duration = ss.Length;
                    GameRuntime.CurrentGlobals.SubtitleController.ShowSubtitle(subtitle);
                }
            });
            Actions.Add(typeof(IssueMission), (e) =>
            {
                if (e is IssueMission mission)
                {
                    if (FPSController.Instance != null)
                    {
                        FPSController.Instance.IssueMission(mission.MissionText);
                    }
                }
            });
            Actions.Add(typeof(IssueBroadcast), (e) => {
                if (BroadcastRecord.Instance != null)
                {
                    BroadcastRecord.Instance.IssueBroadCast((e as IssueBroadcast).BroadCast);
                }
            });
            Actions.Add(typeof(GiveWeaponEvent), (e) =>
            {
                if (e is GiveWeaponEvent GWE)
                {
                    if (FPSController.Instance != null)
                    {
                        var BAG = FPSController.Instance.BagHolder;
                        FPSController.Instance.GiveWeapon(GWE.weapon);
                    }
                }
            });
        }
    }
}
