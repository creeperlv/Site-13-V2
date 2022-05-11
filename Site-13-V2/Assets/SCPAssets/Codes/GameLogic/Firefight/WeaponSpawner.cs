using Site13Kernel.Data;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Firefight
{
    public class WeaponSpawner : MonoBehaviour
    {
        public string PrepareAnimation;
        public string PickedupAnimation;
        public Animator animator;
        public float SpawnTime;
        public Transform Holder;
        public PrefabReference WeaponID;
        void Start()
        {
        
        }
        public void StartSpawnProcess()
        {
            StartCoroutine(SpawnProcess());
        }
        IEnumerator SpawnProcess()
        {
            animator.Play(PrepareAnimation);
            yield return new WaitForSeconds(SpawnTime);
            Spawn();
        }
        void Spawn()
        {
            var g=ObjectGenerator.Instantiate(WeaponID, Holder);
            var r=g.GetComponent<Rigidbody>();
            if (r != null)
            {
                Destroy(r);
            }
            g.transform.localPosition = Vector3.zero;
            var p=g.GetComponentInChildren<Pickupable>();
            if (p != null)
            {
                p.OnPickup = () => {
                    animator.Play(PickedupAnimation);
                    StartCoroutine(SpawnProcess());
                };
            }
        }
    }
}
