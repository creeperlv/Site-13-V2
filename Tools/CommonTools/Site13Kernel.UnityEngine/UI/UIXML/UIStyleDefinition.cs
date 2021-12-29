using System;
using System.Collections.Generic;

namespace Site13Kernel.UI.UIXML
{
    [Serializable]
    public class UIStyleDefinition
    {
        public string StyleName;
        public List<UIElementDefinition> Definitions = new List<UIElementDefinition>();
    }
}
