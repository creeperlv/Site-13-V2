using System;
using UnityEngine.Rendering;

namespace Site13Kernel.SceneBuild.Serializables
{
    [Serializable]
    public class SerializableReflectionProbe : SerializableBase
    {
        public ReflectionProbeRefreshMode RefreshMode;
        public SerializableVector3 Size;
    }
}