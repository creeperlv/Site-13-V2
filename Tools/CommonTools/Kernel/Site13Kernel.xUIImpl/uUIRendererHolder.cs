using Site13Kernel.Core;
using System.Collections;
using UnityEngine;
using xUI.Core.Abstraction;

namespace Site13Kernel.xUIImpl
{
    public class uUIRendererHolder : ControlledBehavior
    {
        public RectTransform transform;
        void Start()
        {
            var re= new uUIRendererEngine();
            re.RootTransform = transform;
            AbstractRenderEngine.CurrentEngine = re;
        }
    }

}
