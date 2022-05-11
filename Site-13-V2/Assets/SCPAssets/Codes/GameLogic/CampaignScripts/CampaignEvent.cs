using CLUNL.Localization;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.AI;
using System;
using UnityEngine;

namespace Site13Kernel.GameLogic.CampaignScripts
{
    [Serializable]
    public class CampaignEvent
    {
        [Header("Execution")]
        public bool isIgnored;
        public bool useTrigger;
        public bool useTimer;
        public bool useSymbol;
        public string TargetSymbol;
        public bool AllowDuplicateExecution;
        public bool Executed;
        public GameObject Trigger;
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
        [Header("Spawn Player")]
        public string PlayerSpawnID="PLAYER";
        [Header("Give Player Weapon")]
        public Weapon TargetWeapon;
        [Header("TriggerGameObject")]
        public GameObject ControlledObject;
        public bool isReverse;
        public bool TargetState;
        [Header("General")]
        public Transform Location;
        [Header("Script")]
        public string ExecutingScript;
        [Header("Scene Related")]
        public string VisibilitySceneName;
        public bool Visibility;
        public string ActiveSceneName;
        [Header("Issue Mission Text")]
        public LocalizedString MissionText;
        #region ExecutorFields
        [HideInInspector]
        public float TimeD;
        #endregion
    }
}
