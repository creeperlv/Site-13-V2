using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using Site13Kernel.UI.HUD;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

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
                    if(HUDBase.Instance!= null)
                    {
                        HUDBase.Instance.IssueMission(mission.MissionText);
                    }
                }
            });
            Actions.Add(typeof(IssueBroadcast), (e) =>
            {
                if (BroadcastRecord.Instance != null)
                {
                    BroadcastRecord.Instance.IssueBroadCast((e as IssueBroadcast).BroadCast);
                }
            });
            Actions.Add(typeof(ToggleHUD), (e) =>
            {
                if (e is ToggleHUD hud)
                {
                    HUDBase.Instance.Show = hud.TargetState;
                }
            });
            Actions.Add(typeof(SpawnAIEvent), (e) =>
            {
                if (e is SpawnAIEvent ai)
                {
                    var __ENTITY_ID=ai.ID;

                    var L = FromSerializableLocation(ai.SpawnLocation);
                    var R=UnityEngine.Random.Range(0, ai.RandomDistance);
                    var Phi=UnityEngine.Random.Range(0, 2 * Mathf.PI);
                    var Pos = L.Position;
                    Pos.x += R * Mathf.Cos(Phi);
                    Pos.z += R * Mathf.Sin(Phi);
                    var agent=AIController.CurrentController.SpawnV2(ai.ID, Pos, L.Rotation.eulerAngles);
                    //agent.agent.routin
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
