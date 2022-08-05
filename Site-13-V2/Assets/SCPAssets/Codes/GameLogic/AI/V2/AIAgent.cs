using CleverCrow.Fluid.BTs.Trees;
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
        public Biped ControlledBiped;
        [SerializeField]
        public BehaviorTree tree;
        Vector3 Goal;
        NavMeshPath path;
        private void Awake()
        {
            tree = new BehaviorTreeBuilder(gameObject).Sequence().Condition("Is crouch", () =>
            {
                return false;
            })
                .Do("Crouch", () =>
            {
                var motion = ControlledBiped._MotionMap["crouch"];
                ControlledBiped.Animator.Play(motion.Trigger, motion.Layer);
                return CleverCrow.Fluid.BTs.Tasks.TaskStatus.Success;
            })
            .End().Build();
        }
        void Update()
        {
            //NMAgent.
        }
        private void OnTriggerStay(Collider other)
        {

        }
        public void GenPath()
        {
            NavMesh.CalculatePath(transform.position, Goal, 1, path);
        }
    }
}
