using Site13Kernel.Data.IO;
using System;
using System.Collections.Generic;

namespace Site13Kernel.Data.Serializables
{
    [Serializable]
    public class SerializableRoutine : IPureData
    {
        public bool useSceneReference;
        public string Name;
        public bool RandomNextStep;
        public List<SerializableRoutineStep> Steps=new List<SerializableRoutineStep>();
    }
}
