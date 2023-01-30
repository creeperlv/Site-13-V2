using Newtonsoft.Json;
using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using xUI.Core.Abstraction;

namespace xUI.Core.UIElements
{
    public class UIElement : IUIElement, ISize, IPosition
    {
        List<IUIElement> _Children = new List<IUIElement>();
        public virtual List<IUIElement> Children { get => _Children; set => _Children = value; }
        [JsonIgnore]
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
        Vector2 _Size;
        public Vector2 Size
        {
            get => _Size;
            set
            {
                SetSize(value);
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
            if (IsInitialized())
            {
                if (_Size != Size)
                {
                    SizeImplementation.SetSize(Size);
                    _Size = Size;
                }
            }
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
    public class xUIButton : UIElement, IClickable, IContent
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

        IContentImpl icimpl = null;
        public void SetIContentImpl(IContentImpl impl)
        {
            if (icimpl != null) return;
            icimpl = impl;
        }

        Site13Event ClickEvent = new Site13Event();

        public object Content { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Site13Event OnClickEvent => ClickEvent;
    }
    public class xUIVerticalStackPanel : UIElement
    {

    }
}
