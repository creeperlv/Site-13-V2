using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data
{
    public class MaterialMatrixDefinition:ControlledBehavior
    {
        public static MaterialMatrixDefinition Instance;
        public MaterialMatrix matrix;
        public Dictionary<MatrixIndex, MaterialCrossDefinition> Definitions;
        public bool UseControlledBehaviorWorkflow;
        void _____()
        {
            Instance= this;
            Definitions = matrix.Matrix.ObtainMap();
        }
        public void Start()
        {
            if (UseControlledBehaviorWorkflow) return;
            _____();
        }
        public override void Init()
        {
            if (UseControlledBehaviorWorkflow) _____();
        }
    }
}
