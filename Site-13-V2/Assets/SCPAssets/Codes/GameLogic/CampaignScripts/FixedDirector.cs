using Site13Kernel.Core.Controllers;
using Site13Kernel.Diagnostics;
using Site13Kernel.GameLogic.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.CampaignScripts
{
    public class FixedDirector : MonoBehaviour
    {
        float TimeCount = 0;
        public List<CampaignEvent> EventList = new List<CampaignEvent>();
        //public GameObject PlayerPrefab;
        public BaseController LevelController;
        public static FixedDirector CurrentDirector;
        void Start()
        {
            CurrentDirector = this;
            foreach (var item in EventList)
            {
                if (item.Trigger != null)
                    if (item.useTrigger)
                        item.Trigger.Callback.Add(() => ExecuteEvent(item));
            }
        }
        public bool isRunning = false;
        IEnumerator Execute(CampaignEvent e)
        {
            yield return new WaitForSeconds(e.OnTriggeredTime);
            RealExecute(e);
        }
        FPSController FPSC;
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

                                var PLAYER = GlobalBioController.CurrentGlobalBioController.Spawn("PLAYER", Vector3.zero, Vector3.zero);
                                PLAYER.transform.GetChild(1).position = e.Location.position;
                                PLAYER.transform.GetChild(1).rotation = e.Location.rotation;
                                FPSC = PLAYER.GetComponentInChildren<FPSController>();

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
                            if (FPSC != null)
                                FPSC.gameObject.SetActive(false);
                        }
                        break;
                    case EventType.EnablePlayer:
                        {
                            if (FPSC != null)
                                FPSC.gameObject.SetActive(true);
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
    [Serializable]
    public class CampaignEvent
    {
        public bool isIgnored;
        public bool useTrigger;
        public bool useTimer;
        public bool AllowDuplicateExecution;
        public bool Executed;
        public EventTrigger Trigger;
        public float OnTriggeredTime;
        public EventType TriggerType;
        [Header("Speak")]
        public AudioSource Audio;
        public string Subtitle;
        [Header("Spawn")]
        public string SpawnID;
        public Vector3 DeltaPosition;
        public Routine Routine;
        public AIState InitState;
        [Header("TriggerGameObject")]
        public GameObject ControlledObject;
        public bool isReverse;
        public bool TargetState;
        [Header("General")]
        public Transform Location;
        [Header("Script")]
        public string ExecutingScript;
        #region ExecutorFields
        [HideInInspector]
        public float TimeD;
        #endregion
    }

    public enum EventType
    {
        Speak, Spawn, Win, Script, SpawnPlayer, TriggerGameObject, DisablePlayer, EnablePlayer
    }
}
