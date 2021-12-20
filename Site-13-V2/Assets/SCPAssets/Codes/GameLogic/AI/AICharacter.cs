using Site13Kernel.Animations;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.FPS;
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
        public string CombatMoveTrigger = "Combat";
        public int CombatMove_HASH = 3;
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

        public GameObject DropItemPrefab;
        public string DropItemID;

        public float RemainingMagzineMax;
        public float RemainingMagzineMin;
        public float RemainingBackupMax;
        public float RemainingBackupMin;

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
                if (DropItemID != "")
                {
                    WeaponPool.CurrentPool.Instantiate(DropItemID, base.transform.position, base.transform.rotation, base.transform.parent);
                }
                else if (DropItemPrefab != null)
                {

                    GameObject.Instantiate(DropItemPrefab, base.transform.position, base.transform.rotation, base.transform.parent);
                }
                Parent.UnregisterRefresh(this);
                return false;
            };
        }
        float TIME_COMBAT = 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            switch (CurrentState)
            {
                case AIState.Walk:
                    {
                        if (GoalRoutine == null) return;
                        if (GoalRoutine.CurrentExecutingGoals.Count == 0) return;

                        if (LastState != CurrentState)
                        {
                            ControlledCharacterAnimator.SetAnimation(Walk_HASH);
                            SetGoal(GoalRoutine.CurrentExecutingGoals[GoalRoutine.Step], WalkSpeed);
                            LastState = CurrentState;
                        }

                        if (Detect(GoalRoutine.CurrentExecutingGoals[GoalRoutine.Step]))
                        {
                            GoalRoutine.Step++;
                            if (GoalRoutine.Step >= GoalRoutine.CurrentExecutingGoals.Count)
                            {
                                if (GoalRoutine.isLoop)
                                {
                                    GoalRoutine.Step = 0;

                                }
                                else
                                {
                                    CurrentState = AIState.Idle;
                                }
                            }
                            if (GoalRoutine.Step < GoalRoutine.CurrentExecutingGoals.Count)
                            {
                                SetGoal(GoalRoutine.CurrentExecutingGoals[GoalRoutine.Step], WalkSpeed);
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
                            ControlledCharacterAnimator.SetAnimation(CombatMove_HASH);
                            LastState = CurrentState;
                        }

                        TIME_COMBAT += DeltaTime;
                        if (TIME_COMBAT > 1)
                        {
                            SetGoal(CombatGoal, WalkSpeed);
                            TIME_COMBAT = 0;
                        }
                        if (Weapon != null)
                        {
                            var angle = Vector3.Angle(Weapon.FirePoint.forward, Weapon.FirePoint.position - CombatGoal.Target.position) - 90;

                            if (Math.Abs(angle) < FireAngleThreshold)
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
                        if (item.TimeD == 0)
                        {
                            {
                                //Play Sound.
                                if (item.audioClips.Count > 0)
                                {
                                    SpeechSource.clip = item.audioClips[UnityEngine.Random.Range((int)0, item.audioClips.Count)];

                                }
                                SpeechSource.Play();
                            }
                        }
                        item.TimeD += DeltaTime;
                        if (item.TimeD > item.LoopWait)
                        {
                            item.TimeD = 0;
                         
                        }
                        break;
                    }
                }

            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetGoal(Goal g, float BaseSpeed = 1)
        {
            Agent.speed = BaseSpeed * g.Speed;
            Agent.destination = g.Target.position;
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
            GoalRoutine = r;
            if (r.CurrentExecutingGoals != null)
                if (r.CurrentExecutingGoals.Count > 0)
                    SetGoal(r.CurrentExecutingGoals[0], CurrentState == AIState.Walk ? WalkSpeed : 1);
            r.Step = 0;
        }
    }
    [Serializable]
    public class Routine
    {
        public bool isLoop;
        public int Step;
        public List<Goal> CurrentExecutingGoals;
    }
    [Serializable]
    public class SpeechCollection
    {
        public AIState State;
        public List<AudioClip> audioClips;
        public bool isLoop;
        public float LoopWait;
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
