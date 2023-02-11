using Site13Kernel.xUIImpl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using xUI.Core.Abstraction;
using xUI.Core.UIElements;

namespace Site13Kernel
{
    public class TestApp : xUIBase
    {
        public override void Start()
        {
            base.Start();
            xUIText text = new xUIText();
            text.Content = "Hello, World!";
           
            AbstractRenderEngine.CurrentEngine.CommitUITree(text);
        }
    }
}
