using Site13Kernel.SceneBuild.Serializables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.SceneBuild
{
    [Serializable]
    public class SceneDescription
    {
        public int SceneSkybox;
        public SerializableColor AmbientColor;
        public SerializableColor ForColor;
        public float Near;
        public float Far;
        public List<SerializableObject> Objects = new List<SerializableObject>();
    }
}