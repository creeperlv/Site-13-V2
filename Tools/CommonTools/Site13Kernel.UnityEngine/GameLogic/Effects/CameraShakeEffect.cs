using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace Site13Kernel.GameLogic.Effects
{
    public class CameraShakeEffect : MonoBehaviour
    {
        public Transform ControlledObject;
        public bool EnableX = true;
        public bool EnableY = true;
        public float Intensity = 0;
        public bool willDiminish;
        public float DiminishIntensity;
        void Update()
        {
            if (Intensity > 0)
            {
                ControlledObject.transform.localPosition = new Vector3(EnableX?Random.Range(-1, 1):0, EnableY?Random.Range(-1, 1):0, 0) * Intensity;
                if (willDiminish) Intensity -= Time.deltaTime * DiminishIntensity;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetShake(float Intensity = 0.1f, bool willDiminish = true, float DiminishIntensity = 0.5f)
        {
            this.Intensity = Intensity;
            this.willDiminish = willDiminish;
            this.DiminishIntensity = DiminishIntensity;
        }
    }

}