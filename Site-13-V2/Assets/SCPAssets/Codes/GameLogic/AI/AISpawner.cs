using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.AI
{
    public class AISpawner : MonoBehaviour
    {
        public Transform TargetPosition;
        public Vector3 DeltaPosition;
        public AISpawnerWorkflow AISpawnerWorkflow;
        public PrefabReference AIPrefab;
        public AIState InitState;
        public Routine InitialRoutine;
        public bool Pause = false;
        public float PreSpan;
        public int SpawnCount;
        public float SpawnSpan;
        void Start()
        {
        
        }
        int CurrentSpawnCount=0;
        float PreSpwanD;
        float SpawnD;
        void Update()
        {
            if (Pause) return;
            if (PreSpwanD > PreSpan)
            {
                switch (AISpawnerWorkflow)
                {
                    case AISpawnerWorkflow.LimitedTime:
                        if(CurrentSpawnCount < SpawnCount)
                        {
                            if (SpawnD != 0)
                            {
                                SpawnD += Time.deltaTime;
                                if(SpawnD > SpawnSpan)SpawnD=0;
                            }
                            else
                            {
                                SpawnOne();
                                CurrentSpawnCount++;
                                SpawnD += Time.deltaTime;
                            }
                        }
                        break;
                    case AISpawnerWorkflow.Constantly:
                        {

                            if (SpawnD != 0)
                            {
                                SpawnD += Time.deltaTime;
                                if (SpawnD > SpawnSpan) SpawnD = 0;
                            }
                            else
                            {
                                SpawnOne();
                                SpawnD += Time.deltaTime;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                PreSpwanD +=Time.deltaTime;
            }
        }
        void SpawnOne()
        {
            var AI = AIController.CurrentController.Spawn(AIPrefab.Key, TargetPosition.position + DeltaPosition, TargetPosition.rotation.eulerAngles);
            AI.CurrentState = InitState;
            AI.GoalState = InitState;
            AI.SetRoutine(InitialRoutine);
        }
    }
    public enum AISpawnerWorkflow
    {
        LimitedTime, Constantly
    }
}
