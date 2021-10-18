
using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class MeleeArea : MonoBehaviour
    {
        public float BaseDamage;
        public bool AllowBackstabDetection;
        public GameObject Holder;
        public bool isDetecting = false;
        private void OnTriggerEnter(Collider collision)
        {
            Hit(collision);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartDetection()
        {
            isDetecting = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StopDetection()
        {
            isDetecting = false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Hit(Collider collision)
        {
            if (isDetecting)
            {
                var DE = collision.gameObject.GetComponent<DamagableEntity>();
                if (DE != null)
                {
                    if (AllowBackstabDetection && DE.CanBeBackstabed)
                    {
                        Vector3 forward = DE.transform.forward;
                        Vector3 toOther = Holder.transform.position - DE.transform.position;
                        if (Vector3.Dot(forward, toOther) < 0)
                        {
                            DE.Die();
                        }
                        else
                        {
                            DE.Damage(BaseDamage);
                        }
                    }
                    else
                    {
                        DE.Damage(BaseDamage);
                    }
                }
            }
        }
    }
}
