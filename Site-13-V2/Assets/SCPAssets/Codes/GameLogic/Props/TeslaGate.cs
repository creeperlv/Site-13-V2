using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel
{
    public class TeslaGate : MonoBehaviour
    {
        public GameObject TeslaArea;
        public GameObject PreTesla;
        public GameObject PreTesla2;
        public GameObject PostTesla;
        public float PreTeslaLength;
        public float PreTesla2Length;
        public float TeslaLength;
        public float PostTeslaLength;
        bool IDLE = true;
        void StartTesla()
        {
            if (IDLE)
            {
                StartCoroutine(Tesla());
            }
        }
        IEnumerator Tesla()
        {
            IDLE = false;
            PreTesla.SetActive(true);
            yield return new WaitForSeconds(PreTeslaLength);
            PreTesla.SetActive(false);
            PreTesla2.SetActive(true);
            yield return new WaitForSeconds(PreTesla2Length);
            PreTesla2.SetActive(false);
            TeslaArea.SetActive(true);
            yield return new WaitForSeconds(TeslaLength);
            TeslaArea.SetActive(false);
            PostTesla.SetActive(true);
            yield return new WaitForSeconds(PostTeslaLength);
            PostTesla.SetActive(false);
            IDLE = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnTriggerEnter(Collider other)
        {
            var DE = other.gameObject.GetComponentInChildren<DamagableEntity>();
            if(DE != null)
            {
                StartTesla();
            }
        }
    }
}
