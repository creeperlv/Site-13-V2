
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
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
            this.gameObject.SetActive(true);
            isDetecting = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StopDetection()
        {
            this.gameObject.SetActive(false);
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
                    if (Holder != null)
                    {
                        if (DE.gameObject == Holder) return;

                        var fps = Holder.GetComponent<FPSController>();
                        if (fps != null)
                        {
                            fps.OnHit();
                        }
                    }

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
