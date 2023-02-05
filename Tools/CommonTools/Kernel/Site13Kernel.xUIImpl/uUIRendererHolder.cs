using Site13Kernel.Core;
using UnityEngine;
using xUI.Core.Abstraction;

namespace Site13Kernel.xUIImpl
{
    public class uUIRendererHolder : ControlledBehavior
    {
        public RectTransform transform;
        void Start()
        {
            AbstractRenderEngine.CurrentEngine = new uUIRendererEngine();
        }
    }

}
