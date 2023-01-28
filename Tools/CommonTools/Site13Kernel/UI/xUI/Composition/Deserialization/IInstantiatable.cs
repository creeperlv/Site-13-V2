using Site13Kernel.UI.xUI.UIElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.UI.xUI.Composition.Deserialization
{
    public interface IInstantiatable
    {
        UIElement Instantiate();
    }
    public class xUIButtonInstantiator : IInstantiatable
    {
        public UIElement Instantiate() => new xUIButton();
    }
    public class xUIWindowInstantiator : IInstantiatable
    {
        public UIElement Instantiate() => new xUIWindow();
    }
    public class xUITextInstantiator : IInstantiatable
    {
        public UIElement Instantiate() => new xUIText();
    }
    public class xUIGridInstantiator : IInstantiatable
    {
        public UIElement Instantiate() => new xUIGrid();
    }
}
