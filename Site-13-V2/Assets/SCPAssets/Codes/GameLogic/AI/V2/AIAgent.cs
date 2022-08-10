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
        public AnimatedCharacter ControlledAnimatedCharacter;
        Vector3 Goal;
        NavMeshPath path;
        private void Awake()
        {
        }
        void Update()
        {
            //NMAgent.
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
