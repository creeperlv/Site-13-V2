using System;
using UnityEngine;

namespace Site13Kernel.SceneBuild.Serializables
{
    [Serializable]
    public class SerializablePointLight : SerializableBase
    {
        public SerializableColor Color;
        public float LightRange;
        public float LightIntensity;
        public LightShadows Shadows;
    }
}