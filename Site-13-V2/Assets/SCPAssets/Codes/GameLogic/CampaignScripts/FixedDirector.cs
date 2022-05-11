using Site13Kernel.Core.Controllers;
using Site13Kernel.Core.Interactives;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.CampaignScripts
{
    public class FixedDirector : MonoBehaviour
    {
        //float TimeCount = 0;
        public List<CampaignEvent> EventList = new List<CampaignEvent>();
        //public GameObject PlayerPrefab;
        public BaseController LevelController;
        public static FixedDirector CurrentDirector;
        public Dictionary<string, bool> Symbols = new Dictionary<string, bool>();
        public PrefabReference DefaultPlayer;
        public Weapon DefaultWeapon;
        public List<Transform> RespawnPoints;
        void Start()
        {
            CurrentDirector = this;
            foreach (var item in EventList)
            {
                if (item.Trigger != null)
                    if (item.useTrigger)
                    {
                        var t = item.Trigger.GetComponent<ITriggerable>();
                        if (t != null)
                        {
                            t.AddCallback(() => ExecuteEvent(item));
                        }
                    }
            }
        }
        public bool isRunning = false;
        IEnumerator Execute(CampaignEvent e)
        {
            yield return new WaitForSeconds(e.OnTriggeredTime);
            RealExecute(e);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSymbol(string Name)
        {
            var __key = Name.ToUpper();
            Symbols.Add(__key, true);
            foreach (var item in EventList)
            {
                if (item.useSymbol)
                {
                    if (item.TargetSymbol.ToUpper() == __key)
                    {
                        StartCoroutine(Execute(item));
                    }
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void RealExecute(CampaignEvent e)
        {
            try
            {
                switch (e.TriggerType)
                {
                    case EventType.Speak:
                        break;
                    case EventType.Spawn:
                        {

                            var AI = AIController.CurrentController.Spawn(e.SpawnID, e.Location.position + e.DeltaPosition, e.Location.rotation.eulerAngles);
                            AI.CurrentState = e.InitState;
                            AI.GoalState = e.InitState;
                            AI.SetRoutine(e.Routine);
                        }
                        break;
                    case EventType.Win:
                        {
                            CampaignDirector.CurrentDirector.Win();
                        }
                        break;
                    case EventType.SpawnPlayer:
                        {
                            try
                            {

                                var PLAYER = GlobalBioController.CurrentGlobalBioController.Spawn(e.PlayerSpawnID, Vector3.zero, Vector3.zero);
                                PLAYER.transform.GetChild(1).position = e.Location.position;
                                PLAYER.transform.GetChild(1).rotation = e.Location.rotation;
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
                        break;
                    case EventType.DisablePlayer:
                        {
                            if (FPSController.Instance != null)
                                FPSController.Instance.gameObject.SetActive(false);
                        }
                        break;
                    case EventType.EnablePlayer:
                        {
                            if (FPSController.Instance != null)
                                FPSController.Instance.gameObject.SetActive(true);
                        }
                        break;
                    case EventType.Script:
                        {
                            ScriptEngine.Execute(e.ExecutingScript);
                        }
                        break;
                    case EventType.TriggerGameObject:
                        {
                            if (e.isReverse)
                            {
                                e.ControlledObject.SetActive(!e.ControlledObject.activeSelf);
                            }
                            else
                            {
                                e.ControlledObject.SetActive(e.TargetState);
                            }
                        }
                        break;
                    case EventType.GivePlayerWeapon:
                        {
                            if (FPSController.Instance != null)
                            {
                                var BAG = FPSController.Instance.BagHolder;
                                FPSController.Instance.GiveWeapon(e.TargetWeapon);
                                //TODO
                            }
                        }
                        break;
                    case EventType.SetPlayerTransform:
                        {
                            if (FPSController.Instance != null)
                            {
                                FPSController.Instance.transform.position = e.Location.position;
                                FPSController.Instance.transform.rotation = e.Location.rotation;
                            }
                        }
                        break;
                    case EventType.SceneVisibility:
                        {
                            var ID = SceneUtility.LookUp(e.VisibilitySceneName);
                            if (ID != -1)
                            {
                                if (e.Visibility)
                                {
                                    SceneLoader.Instance.ShowScene(ID);
                                }
                                else
                                {
                                    SceneLoader.Instance.HideScene(ID);
                                }
                            }
                            else
                            {
                                Debugger.CurrentDebugger.LogError($"FixedDirector: VisibilitySceneName({e.VisibilitySceneName}) lookup failed.");
                            }
                        }
                        break;
                    case EventType.SceneActive:
                        {
                            var ID = SceneUtility.LookUp(e.ActiveSceneName);
                            if (ID != -1)
                            {
                                SceneLoader.Instance.SetActive(ID);
                            }
                            else
                            {
                                Debugger.CurrentDebugger.LogError($"FixedDirector: ActiveSceneName({e.ActiveSceneName}) lookup failed.");
                            }
                        }
                        break;
                    case EventType.IssueMission:
                        {
                            if (FPSController.Instance != null)
                            {
                                FPSController.Instance.IssueMission(e.MissionText);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception _e)
            {
                Debugger.CurrentDebugger.LogError(_e);
            }
            e.Executed = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ExecuteEvent(CampaignEvent e)
        {
            if (e.isIgnored) return;
            if (e.Executed && !e.AllowDuplicateExecution) return;
            if (e.useTrigger && e.useTimer)
            {
                StartCoroutine(Execute(e));
            }
            else
            {
                RealExecute(e);
            }
        }
        void Update()
        {
            float TD = Time.deltaTime;
            if (isRunning == false) return;
            foreach (var item in EventList)
            {
                if (item.useTrigger && !item.useTimer) continue;
                if (item.useSymbol) continue;
                bool EXE = true;
                if (item.useTrigger) continue;
                if (item.useTimer)
                {
                    item.TimeD += TD;
                    if (item.TimeD > item.OnTriggeredTime)
                    {
                        EXE = true;
                        if (item.AllowDuplicateExecution) item.TimeD = 0;
                    }
                    else EXE = false;
                }
                EXE = EXE & (!item.Executed || item.AllowDuplicateExecution);
                //if (!EXE)
                //{
                //    if (item.AllowDuplicateExecution)
                //    {
                //        EXE = true;
                //    }
                //}
                if (EXE)
                {
                    ExecuteEvent(item);
                }
            }
        }
    }

    public enum EventType
    {
        Speak, Spawn, Win, Script, SpawnPlayer, TriggerGameObject, DisablePlayer, EnablePlayer, GivePlayerWeapon, SceneVisibility, CheckPoint, SceneActive, SetPlayerTransform,IssueMission
    }
}
