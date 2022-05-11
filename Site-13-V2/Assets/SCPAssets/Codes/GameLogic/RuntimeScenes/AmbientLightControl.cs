using UnityEngine;
namespace Site13Kernel.GameLogic.RuntimeScenes
{
    public class AmbientLightControl : MonoBehaviour
    {
        [ColorUsage(false,true)]
        public Color Color;
        private Color LastColor;
        private void Update()
        {
            if (LastColor != Color)
            {
                LastColor = Color;
                RenderSettings.ambientLight = Color;
            }
        }
    }

}

