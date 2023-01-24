using Site13Kernel.Core;
using Site13Kernel.UI.xUI.Abstraction;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Site13Kernel.UI.xUI.UIElements
{
    public class UIElement : IUIElement, ISize, IPosition
    {
        public List<UIElement> Children { get; set; }
        public UIElement Parent { get; set; }
        public virtual string Name { get; set; }

        public void Initialize()
        {

        }
        internal IUIElementImplementation ElementImplementation;
        public void SetIUIElementImplementation(IUIElementImplementation implementation)
        {
            ElementImplementation = implementation;
        }

        public virtual void SetPosition(Vector2 Position)
        {
        }
        internal ISizeImplementation SizeImplementation;
        public virtual void SetISizeImplementation(ISizeImplementation implementation)
        {
            SizeImplementation = implementation;
        }

        public virtual void SetSize(Vector2 Size)
        {
            SizeImplementation.SetSize(Size);
        }

        public virtual void SetProperty(string name, object value)
        {

        }

        public virtual object GetProperty(string name)
        {
            return null;
        }

        public virtual void SetIPositionImplementation(IPositionImplementation implementation)
        {

        }

        public virtual void SetPositionDataOnly(Vector2 Position)
        {

        }

        public virtual void SetSizeDataOnly(Vector2 Size)
        {

        }
    }
    public class xWindow : UIElement
    {
        public string Title;

    }
    public class xUIText : UIElement
    {
        public string Content;
    }
    public class xUIButton : UIElement, IClickable
    {
        public void OnClick()
        {
            ClickEvent.Invoke();
        }
        internal IClickableImplementation clickableImplementation;
        public void SetIClickableImplementation(IClickableImplementation implementation)
        {
            clickableImplementation = implementation;
            clickableImplementation.SetOnClick(OnClick);
        }

        public void AddEvent(Site13Event actions)
        {
            ClickEvent.ConnectAfterEnd(actions);
        }

        public Site13Event ClickEvent;
    }
    public class xUIVerticalStackPanel : UIElement
    {

    }
}
