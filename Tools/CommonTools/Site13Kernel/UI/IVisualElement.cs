using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.UI
{
    public interface IVisualElement : ILayoutable
    {
        void Show();
        void Hide();
    }
    public interface ILayoutable
    {
        Bounds GetBounds();
    }
}
