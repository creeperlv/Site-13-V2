using CleverCrow.Fluid.BTs.Trees;
using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.BT.Nodes.Actions;
using Site13Kernel.GameLogic.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Site13Kernel.GameLogic.AI.V2
{
    public class AIAgent : MonoBehaviour
    {
        //public NavMeshAgent NMAgent;
        public AnimatedCharacter ControlledAnimatedCharacter;
        public BioEntity ControlledEntity;
        public float SightRange;
        public float AttackRange;
        public float AttackStopRange;
        public float LowHealth;
        public float WalkSpeed = 5;
        public float ChaseSpeed;
        public string CombatGroupID;
        public BehaviorMode CurrentMode = BehaviorMode.Goal;
        public BioCollector Collector;
        public Directors.AIState AIState = Directors.AIState.Idle;
        public Directors.AIState CurrentAIState = Directors.AIState.Idle;
        public NavMeshAgent NavAgent;
        public float RePathTimeInterval = 1;
        public KVList<Directors.AIState, string> Motions;
        Vector3 Goal;
        NavMeshPath path;
        public float BlockActionCountDown = 0;
        float TimeCumulation = 0;
        internal bool isHit = false;
        private void Awake()
        {
            ControlledEntity.OnTakingDamage = (_, _, _, _, _) =>
            {
                isHit = true;
            };
        }
        public void OnFrame(float DT)
        {
            TimeCumulation += DT;
            if (TimeCumulation > RePathTimeInterval)
            {
                TimeCumulation = 0;
                RePath();
            }
        }
        public void SetAIState(Directors.AIState state)
        {
            if (state != CurrentAIState)
            {
                foreach (var item in Motions.PrefabDefinitions)
                {
                    if (item.Key == state)
                    {
                        ControlledAnimatedCharacter.PlayMotion(item.Value, 0);
                        break;
                    }
                }
            }
            else
                CurrentAIState = state;
        }
        Transform FollowGoal;
        Vector3 _LastPos;
        public void RePath()
        {
            float SD = 1;
            switch (CurrentMode)
            {
                case BehaviorMode.Goal:
                    {
                        if (AIState == Directors.AIState.Goal)
                        {
                            NavAgent.isStopped = false;
                            SetAIState(AIState);
                        }
                        else if (AIState == Directors.AIState.Idle)
                        {
                            FollowGoal = null;
                            NavAgent.isStopped = true;
                            SetAIState(AIState);
                        }
                    }
                    break;
                case BehaviorMode.Fight:
                    {
                        NavAgent.isStopped = false;
                        FollowGoal = Collector.LastClosestFoe.transform;
                        SetAIState(Directors.AIState.Combat);
                        SD = AttackStopRange;
                    }
                    break;
                case BehaviorMode.Search:
                    break;
                case BehaviorMode.Hide:
                    break;
                default:
                    break;
            }
            if (FollowGoal != null)
            {
                if (NavAgent.velocity.magnitude < 0.1)
                    if (_LastPos != FollowGoal.position)
                    {
                        _LastPos = FollowGoal.position;
                        NavAgent.SetDestination(_LastPos);
                        NavAgent.speed = WalkSpeed;
                        NavAgent.stoppingDistance = SD;
                    }

            }
        }
        public bool isAwareOfEnemy;
        public void CheckEnemies()
        {
            if (Collector.LastClosestFoe != null)
            {
                float Dis = (Collector.LastClosestFoe.transform.position - ControlledEntity.transform.position).magnitude;
                if (Dis < SightRange)
                {
                    isAwareOfEnemy = true;
                }
                else
                {
                    isAwareOfEnemy = false;
                }
            }
        }
        bool willCrouch = false;
        private void OnTriggerStay(Collider other)
        {
            var area = other.GetComponent<MapArea>();
            if (area != null)
            {
                switch (area.areaType)
                {
                    case MapAreaType.Crouch:
                        willCrouch = true;
                        break;
                    case MapAreaType.Blindage:
                        break;
                    default:
                        willCrouch = false;
                        break;
                }
            }
        }
        public void GenPath()
        {
            NavMesh.CalculatePath(transform.position, Goal, 1, path);
        }
    }
}
