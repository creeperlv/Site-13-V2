using System.Collections.Generic;
using UnityEngine;
namespace Site13Kernel.GameLogic.RuntimeScenes
{
    public class SkyboxControl : MonoBehaviour
    {
        public List<Material> Skyboxes;
        public int Using;
        internal int _Using=-1;
        private void Update()
        {
            if (_Using != Using)
            {
                _Using = Using;
                RenderSettings.skybox = Skyboxes[Using];
            }
        }
    }

}

