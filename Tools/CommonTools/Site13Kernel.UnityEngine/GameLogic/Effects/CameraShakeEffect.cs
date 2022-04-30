using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace Site13Kernel.GameLogic.Effects
{
    public class CameraShakeEffect : MonoBehaviour
    {
        public Transform ControlledObject;
        public float HorizontalBaseIntensity = 1;
        public float VerticalBaseIntensity = 1;
        public float Intensity = 0;
        public bool willDiminish;
        public float DiminishIntensity;
        public float RotationShakeSpeed = 25;
        public bool RotationMode = false;
        Vector3 DesignatedRoataion = Vector3.zero;
        void Update()
        {
            if (Intensity > 0)
            {
                if (RotationMode)
                {
                    var ConvertedRotation = ControlledObject.transform.localEulerAngles;
                    if (ConvertedRotation.x > 180) ConvertedRotation.x -= 360;
                    if (ConvertedRotation.y > 180) ConvertedRotation.y -= 360;
                    ControlledObject.localEulerAngles = MathUtilities.SmoothClose(ConvertedRotation, DesignatedRoataion, Time.deltaTime * RotationShakeSpeed);
                    if (MathUtilities.WithInRange(ConvertedRotation, DesignatedRoataion, 0.05f))
                    {
                        if (DesignatedRoataion == Vector3.zero)
                        {
                            DesignatedRoataion = new Vector3(Random.Range(-1, 1) * VerticalBaseIntensity, Random.Range(-1, 1) * HorizontalBaseIntensity, 0) * Intensity;
                        }
                        else
                        {
                            DesignatedRoataion = Vector3.zero;
                        }
                    }
                }
                else
                {
                    ControlledObject.transform.localPosition = new Vector3(HorizontalBaseIntensity * Random.Range(-1, 1), VerticalBaseIntensity * Random.Range(-1, 1), 0) * Intensity;
                }
                if (willDiminish) Intensity -= Time.deltaTime * DiminishIntensity;
            }
            else
            {
                if (RotationMode)
                {
                    var ConvertedRotation = ControlledObject.transform.localEulerAngles;
                    if (ConvertedRotation.x > 180) ConvertedRotation.x -= 360;
                    if (ConvertedRotation.y > 180) ConvertedRotation.y -= 360;
                    ControlledObject.localEulerAngles = MathUtilities.SmoothClose(ConvertedRotation, Vector3.zero, Time.deltaTime * RotationShakeSpeed);
                }
                else
                {
                    if (ControlledObject.transform.localPosition != Vector3.zero)
                        ControlledObject.transform.localPosition = Vector3.zero;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetShake(float Intensity = 0.1f, bool willDiminish = true, float DiminishIntensity = 0.5f, bool RotationMode = false, float RotationShakeSpeed = 25f, float HorizontalBaseIntensity = 1, float VerticalBaseIntensity = 1)
        {
            this.Intensity = Intensity;
            this.willDiminish = willDiminish;
            this.DiminishIntensity = DiminishIntensity;
            this.RotationMode = RotationMode;
            this.HorizontalBaseIntensity = HorizontalBaseIntensity;
            this.VerticalBaseIntensity = VerticalBaseIntensity;
            if (RotationMode)
            {
                this.RotationShakeSpeed = RotationShakeSpeed;
                DesignatedRoataion = new Vector3(Random.Range(-1, 1) * VerticalBaseIntensity, Random.Range(-1, 1) * HorizontalBaseIntensity, 0) * Intensity;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetShake(float Intensity = 0.1f, bool willDiminish = true, float DiminishIntensity = 0.5f, float HorizontalBaseIntensity = 1, float VerticalBaseIntensity = 1)
        {
            SetShake(Intensity, willDiminish, DiminishIntensity, false, 25, HorizontalBaseIntensity, VerticalBaseIntensity);
        }
    }

}