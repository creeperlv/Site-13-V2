using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Site13Kernel.GameLogic.AI.V2
{
    public class AIAgent : MonoBehaviour
    {
        //public NavMeshAgent NMAgent;
        Vector3 Goal;
        NavMeshPath path = new NavMeshPath();
        void Update()
        {
            //NMAgent.
        }
        public void GenPath()
        {
            NavMesh.CalculatePath(transform.position, Goal, 1, path);
        }
    }
}
