using Site13Kernel.Animations;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.UI;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace Site13Kernel.GameLogic.AI
{
    public class AICharacter : ControlledBehavior
    {
        public Transform ControlledCharacter;
        public CompatibleAnimator ControlledCharacterAnimator;
        public BioEntity BioEntity;
        public string WalkTrigger = "Walk";
        public int Walk_HASH = 0;
        public string IdleTrigger = "Idle";
        public int Idle_HASH = 1;
        public string EscapeTrigger = "Escape";
        public int Escaped_HASH = 2;
        public float Escape_P = 0.1f;
        public string CombatMoveTrigger = "Combat_W";
        public int CombatMove_HASH = 3;
        public string CombatStopTrigger = "Combat_S";
        public int CombatStop_HASH = 5;
        public string ThrowGrenadeTrigger = "Throw";
        public int Throw_HASH = 4;
        public float Throw_P = 0.1f;
        public new AIController Parent;
        public AIState LastState = AIState.None;
        public AIState CurrentState;
        public AIState GoalState;

        public NavMeshAgent Agent;

        public AudioSource SpeechSource;

        [Header("Death")]// Yep, this is death.
        public List<DeathDropItem> deathDropItems;
        public GameObject DropItemPrefab;
        public string DropItemID;

        public float RemainingMagzineMax;
        public float RemainingMagzineMin;
        public float RemainingBackupMax;
        public float RemainingBackupMin;

        public string ScoringID;

        [Header("Combat Behavior")]
        public Goal CombatGoal;
        /// <summary>
        /// Stop firing weapon, and instead, chase.
        /// </summary>
        public float SightRange;
        /// <summary>
        /// Back to normal routine.
        /// </summary>
        public float ChaseStopRange;
        public float CombatStopDistance;
        public float ChaseSpeed;
        public float CombatMoveSpeed = 2;

        public float FireAngleThreshold;

        public EscapeReason EscapeReason;

        public BasicWeapon Weapon;

        public float WalkSpeed;

        public BioCollector Collector;

        public List<SpeechCollection> SpeechCollections;
        public Routine GoalRoutine;
        bool Died = false;
        public override void Init()
        {
            BioEntity.OnDie = () =>
            {
                if (Died) return false;
                Died = true;
                if (ScoringID != "")
                    ScoreBoard.Count(ScoringID);
                if (deathDropItems.Count > 0)
                {
                    foreach (var item in deathDropItems)
                    {
                        var p = UnityEngine.Random.Range(0f, 1f);
                        if (p < item.Probability)
                        {
                            if (item.isWeapon)
                            {
                                (var _G, _) = WeaponPool.CurrentPool.Instantiate(item.ItemID.Key, base.transform.position, base.transform.rotation, base.transform.parent);
                                var SD = _G.AddComponent<SelfDestruction>();
                                SD.Time = 30;
                            }
                            else
                            {
                                var _G = Utilities.ObjectGenerator.Instantiate(item.ItemID, base.transform.position, base.transform.rotation, base.transform.parent);
                                var SD = _G.AddComponent<SelfDestruction>();
                                SD.Time = 30;
                            }
                        }
                    }
                }
                else
                {
                    if (DropItemID != "")
                    {
                        WeaponPool.CurrentPool.Instantiate(DropItemID, base.transform.position, base.transform.rotation, base.transform.parent);
                    }
                    else if (DropItemPrefab != null)
                    {

                        GameObject.Instantiate(DropItemPrefab, base.transform.position, base.transform.rotation, base.transform.parent);
                    }
                }
                Parent.UnregisterRefresh(this);
                return false;
            };
        }
        float TIME_COMBAT = 0;
        bool isNotFirstSpot = false;
        bool isSetDestSuccessed = false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (!Agent.isOnNavMesh)
            {
                Agent.enabled = false;
                Agent.enabled = true;
                return;
            }
            switch (CurrentState)
            {
                case AIState.Walk:
                    {
                        if (GoalRoutine == null) return;
                        if (GoalRoutine.Steps.Count == 0) return;

                        if (LastState != CurrentState)
                        {
                            SetGoal(GoalRoutine.Steps[GoalRoutine.CurrentStep], WalkSpeed);
                            LastState = CurrentState;
                        }
                        if (ControlledCharacterAnimator.CurrentClip != Walk_HASH)
                            ControlledCharacterAnimator.SetAnimation(Walk_HASH);

                        if (Detect(GoalRoutine.Steps[GoalRoutine.CurrentStep]))
                        {
                            if (GoalRoutine.RandomizeNextHop)
                            {
                                var (goal,index)=Maths.ObtainOneWithIndex(GoalRoutine.Steps);
                                GoalRoutine.CurrentStep = index;
                                SetGoal(goal, WalkSpeed);
                            }
                            else
                            {
                                GoalRoutine.CurrentStep++;
                                if (GoalRoutine.CurrentStep >= GoalRoutine.Steps.Count)
                                {
                                    if (GoalRoutine.Loop)
                                    {
                                        GoalRoutine.CurrentStep = 0;

                                    }
                                    else
                                    {
                                        CurrentState = AIState.Idle;
                                    }
                                }
                                if (GoalRoutine.CurrentStep < GoalRoutine.Steps.Count)
                                {
                                    SetGoal(GoalRoutine.Steps[GoalRoutine.CurrentStep], WalkSpeed);
                                }
                            }
                            //ControlledCharacterAnimator.SetAnimation(Walk_HASH);
                        }
                    }
                    break;
                case AIState.Idle:
                    {

                        if (LastState != CurrentState)
                        {
                            ControlledCharacterAnimator.SetAnimation(Idle_HASH);
                            LastState = CurrentState;
                        }
                    }
                    break;
                case AIState.Escape:
                    {

                        if (LastState != CurrentState)
                        {
                            ControlledCharacterAnimator.SetAnimation(Escaped_HASH);
                            LastState = CurrentState;
                        }
                    }
                    break;
                case AIState.Combat:
                    {
                        if (LastState != CurrentState)
                        {
                            LastState = CurrentState;
                        }
                        if (ControlledCharacterAnimator.CurrentClip != CombatMove_HASH)
                            ControlledCharacterAnimator.SetAnimation(CombatMove_HASH);
                        if (isNotFirstSpot) ControlledCharacterAnimator.ControlledAnimator.SetBool("FirstSpot", false);
                        isNotFirstSpot = true;
                        TIME_COMBAT += DeltaTime;
                        if (TIME_COMBAT > 1)
                        {
                            if (CombatGoal.Target != null)
                                SetGoal(CombatGoal, WalkSpeed);

                            TIME_COMBAT = 0;
                        }
                        if (CombatGoal.Target != null)
                            if (Weapon != null)
                            {
                                var angle = Vector3.Angle(Weapon.FirePoint.forward, Weapon.FirePoint.position - CombatGoal.Target.position);
                                var _angle = Math.Abs(angle);
                                if (90 - FireAngleThreshold < _angle && _angle < 90 + FireAngleThreshold)
                                {
                                    Weapon.Fire();
                                }
                                else
                                {
                                    Weapon.Unfire();
                                }
                                Weapon.OnFrame(DeltaTime, UnscaledDeltaTime);
                            }
                    }
                    break;
                case AIState.Chase:
                    {
                        if (LastState != CurrentState)
                        {
                            LastState = CurrentState;
                        }

                    }
                    break;
                default:
                    break;
            }
            if (GoalPos != Vector3.zero)
                if (!isSetDestSuccessed)
                {
                    isSetDestSuccessed = Agent.SetDestination(GoalPos);
                }
            // State Change?

            {
                if (CurrentState == AIState.Chase)
                {
                    if (Collector.LastClosestFoe == null)
                    {
                        CurrentState = AIState.Walk;

                    }
                    else
                    {
                        float Dis = (transform.position - Collector.LastClosestFoe.transform.position).magnitude;
                        if (Dis > ChaseStopRange)
                        {
                            CurrentState = AIState.Walk;
                            Collector.LastClosestFoeDistance = ChaseStopRange + 1;
                        }
                    }

                }
                else
                if (CurrentState == AIState.Combat)
                {
                    if (CombatGoal != null)
                    {
                        if (CombatGoal.Target == null)
                            CurrentState = AIState.Walk;
                    }
                    if (Collector.LastClosestFoe == null)
                    {
                        CurrentState = AIState.Walk;

                    }
                    else
                    {
                        float Dis = (transform.position - Collector.LastClosestFoe.transform.position).magnitude;
                        if (Dis > SightRange)
                        {
                            if (Dis < ChaseStopRange)
                            {
                                CurrentState = AIState.Chase;

                            }
                            else
                            {
                                CurrentState = GoalState;
                                Collector.LastClosestFoeDistance = ChaseStopRange + 1;

                            }
                        }
                    }
                }
                else if (CurrentState == AIState.Walk || CurrentState == AIState.Idle)
                {
                    if (Collector.LastClosestFoe != null)
                    {
                        if (Collector.LastClosestFoeDistance < SightRange)
                        {
                            CurrentState = AIState.Combat;
                            CombatGoal = new Goal { Target = Collector.LastClosestFoe.transform, Speed = CombatMoveSpeed, Range = 3 };
                            SetGoal(CombatGoal, WalkSpeed);
                        }
                    }
                }
            }

            // Voice Play?

            {
                foreach (var item in SpeechCollections)
                {
                    if (item.State == CurrentState)
                    {
                        if (item.TimeD == -1)
                        {
                            item.LoopWaitRandom= UnityEngine.Random.Range(0, item.LoopWaitRandomMax);
                            item.TimeD = 0.01f;
                        }
                        if (item.TimeD == 0)
                        {
                            {
                                //Play Sound.
                                if (item.audioClips.Count > 0)
                                {
                                    SpeechSource.clip = Maths.ObtainOne(item.audioClips);

                                }
                                SpeechSource.Play();
                            }
                        }
                        item.TimeD += DeltaTime;
                        if (item.TimeD > item.LoopWait+item.LoopWaitRandom)
                        {
                            item.TimeD = 0;
                            item.LoopWaitRandom= UnityEngine.Random.Range(0, item.LoopWaitRandomMax);
                        }
                        break;
                    }
                }

            }
        }
        Vector3 GoalPos;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetGoal(Goal g, float BaseSpeed = 1)
        {
            Agent.speed = BaseSpeed * g.Speed;
            GoalPos = g.Target.position;
            return isSetDestSuccessed = Agent.SetDestination(GoalPos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <returns>If reaches the point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Detect(Goal g)
        {

            if ((ControlledCharacter.position - g.Target.position).magnitude < g.Range)
            {
                //                Agent.stoppingDistance = g.Range;
                return true;
            }
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRoutine(Routine r)
        {
            GoalRoutine = r.Duplicate() as Routine;
            if (r.Steps != null)
                if (r.Steps.Count > 0)
                    SetGoal(r.Steps[0], CurrentState == AIState.Walk ? WalkSpeed : 1);
            r.CurrentStep = 0;
        }
    }
    [Serializable]
    public class DeathDropItem
    {
        public bool isWeapon = true;
        public PrefabReference ItemID;
        public float Probability = 1;
    }
    //[Serializable]
    //public class Routine
    //{
    //    public bool isLoop;
    //    public bool isRandomNextGoal=false;
    //    public int Step;
    //    public List<Goal> CurrentExecutingGoals;
    //    public Routine Duplicate()
    //    {
    //        return new Routine { isLoop = isLoop, Step = Step,isRandomNextGoal= isRandomNextGoal, CurrentExecutingGoals = CurrentExecutingGoals };
    //    }
    //}
    [Serializable]
    public class SpeechCollection
    {
        public AIState State;
        public List<AudioClip> audioClips;
        public bool isLoop;
        public float LoopWait;
        public float LoopWaitRandomMax;
        [HideInInspector]
        public float LoopWaitRandom;
        public float TimeD;
    }
    public enum EscapeReason
    {
        LowHealth, LeaderDead, None
    }
    public enum AIState
    {
        None, Walk, Idle, Escape, Combat, Chase
    }
}
