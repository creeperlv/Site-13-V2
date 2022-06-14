using UnityEngine;
namespace Site13Kernel.GameLogic.RuntimeScenes
{
    public class AmbientLightControl : MonoBehaviour
    {
        public static AmbientLightControl Instance;
        [ColorUsage(false,true)]
        public Color Color;
        private Color LastColor;
        public void Start()
        {
            Instance = this;
        }
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

