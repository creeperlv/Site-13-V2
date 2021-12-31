using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.UI
{
    public interface IVisualElement : ILayoutable
    {
        Visibility Visibility { get; set; }
        void Show();
        void Hide();
        void Collapse();
    }
    public enum Visibility
    {
        Visible, Hidden, Collapsed
    }
    public interface ILayoutable
    {
        void Size();
    }
}
