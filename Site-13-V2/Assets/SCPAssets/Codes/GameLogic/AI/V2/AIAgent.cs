using CleverCrow.Fluid.BTs.Trees;
using Site13Kernel.Core;
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
        public float LowHealth;
        public BehaviorMode CurrentMode = BehaviorMode.Goal;
        public BioCollector Collector;
        Vector3 Goal;
        NavMeshPath path;
        public float BlockActionCountDown = 0;
        internal bool isHit = false;
        private void Awake()
        {
            ControlledEntity.OnTakingDamage =(_, _, _, _, _) => {
                isHit = true;
            };
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
