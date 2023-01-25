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
        bool _isEnabled;
        bool _isHitEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                ElementImplementation.SetIsEnable(value);
            }
        }
        public bool IsHitEnabled
        {
            get => _isHitEnabled;
            set
            {
                _isHitEnabled = value;
                ElementImplementation.SetHit(value);
            }
        }
        bool _inited = false;
        public virtual void Initialize()
        {
            _inited = true;
        }

        public bool IsInitialized()
        {
            return _inited;
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

        public void SetHitEnabledDataOnly(bool hitEnabled)
        {
            throw new NotImplementedException();
        }
    }
    public class xUIWindow : UIElement
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
    public class xUIGrid : UIElement, IxUIContainer
    {
        public void Add(object content)
        {
            throw new NotImplementedException();
        }

        public void Remove(object content)
        {
            throw new NotImplementedException();
        }
    }
    public class xUIVerticalStackPanel : UIElement
    {

    }
}
