using System;
using System.Collections.Generic;
using System.Text;
using xUI.Core.UIElements;

namespace xUI.Core.Composition.Deserialization
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
    public class xUISolidColorRectangleInstantiator : IInstantiatable
    {
        public UIElement Instantiate() => new xUISolidColorRectangle();
    }
}
