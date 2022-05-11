using Site13Kernel.GameLogic.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Firefight
{
    public class FirefightSpawner : MonoBehaviour
    {
        public float SelfDestruction = 1f;
        public List<AISpawner> ControlledSpawner;
        // Start is called before the first frame update
        void Start()
        {
            if (SelfDestruction != -1)
                StartCoroutine(Life());
        }
        IEnumerator Life()
        {
            yield return new WaitForSeconds(SelfDestruction);
            Destroy(gameObject);
        }
        public void StartSpawn()
        {
            foreach (var item in ControlledSpawner)
            {
                item.Pause = false;
            }
        }
        public void SetRoute(List<Routine> routine)
        {
            foreach (var item in ControlledSpawner)
            {
                item.InitialRoutine = routine[Random.Range(0, routine.Count)].Duplicate();
                item.InitialRoutine.Step= Random.Range(0, item.InitialRoutine.CurrentExecutingGoals.Count);
            }
        }
    }
}
