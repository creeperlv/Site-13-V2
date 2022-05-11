using System;

namespace Site13Kernel.UI.Forge
{
    [Serializable]
    public class ForgeEditorDefinition
    {
        public string TypeName;
        public ForgeComponentEditorBase TargetEditor;
    }
}