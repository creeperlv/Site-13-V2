using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Site13Kernel
{
    public class ControlledCameraShadow : MonoBehaviour
    {
        void Start()
        {
            var cam = this.gameObject.GetComponent<UniversalAdditionalCameraData>();
            if (cam != null)
            {
                cam.renderShadows = Settings.CurrentSettings.RenderShadow;
            }
        }
    }
}
