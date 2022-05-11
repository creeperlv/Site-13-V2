using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic
{
    public class SelfDestruction : MonoBehaviour
    {
        public float Time;

        IEnumerator Start()
        {
            yield return null;
            StartCoroutine(Destruction());
        }
        IEnumerator Destruction()
        {
            yield return new WaitForSeconds(Time);
            Destroy(gameObject);
        }
    }
}
