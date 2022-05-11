using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Site13Kernel.GameLogic.RuntimeScenes
{
    public class EnforceBGOnEnable : MonoBehaviour
    {
        public SkyboxControl ControlledSkybox;
        private void OnEnable()
        {
            ControlledSkybox._Using = -1;
        }
    }

}