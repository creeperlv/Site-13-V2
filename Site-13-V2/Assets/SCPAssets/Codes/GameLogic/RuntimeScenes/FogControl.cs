using System.Collections;
using UnityEngine;
namespace Site13Kernel.GameLogic.RuntimeScenes
{
    public class FogControl : MonoBehaviour
    {
        public static FogControl Instance;
        public Color Fog;
        public float Far;
        private float LastFar=-1;
        public float Near;
        private float LastNear=-1;
        private Color LastFog;
        private void Start()
        {
            Instance = this;
        }
        // Update is called once per frame
        void Update()
        {
            if (LastFog != Fog)
            {
                RenderSettings.fogColor = Fog;
                LastFog = Fog;
            }
            if (LastFar != Far)
            {
                RenderSettings.fogEndDistance = Far;
                LastFar = Far;
            }
            if (LastNear != Near)
            {
                RenderSettings.fogStartDistance = Near;
                LastFar = Far;
            }
        }
    }

}

