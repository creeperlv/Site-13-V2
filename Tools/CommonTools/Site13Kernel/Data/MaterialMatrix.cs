using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data
{
    [Serializable]
    public class MaterialMatrix
    {
        public KVList<MatrixIndex, MaterialCrossDefinition> Matrix=new KVList<MatrixIndex, MaterialCrossDefinition>();
    }
    [Serializable]
    public class MaterialCrossDefinition
    {
        public int SoundID;
        public int EffectID;
    }
    [Serializable]
    public struct MatrixIndex
    {
        public int X;
        public int Y;
    }
}
