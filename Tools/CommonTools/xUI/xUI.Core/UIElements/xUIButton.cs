using Site13Kernel.Core;
using System;
using xUI.Core.Abstraction;

namespace xUI.Core.UIElements
{
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
        object? _Content = null;
        public object Content
        {
            get => _Content;
            set
            {
                if (_Content != value)
                {
                    _Content = value;
                    if (IsInitialized())
                    {
                        Initialize();
                    }
                }
            }
        }
        UIElement? RealContent = null;
        public override void Initialize()
        {
            if (_Content != null)
            {
                if (_Content != RealContent)
                {
                    if (RealContent != null)
                    {
                        AbstractRenderEngine.CurrentEngine.RemoveUITree(RealContent);
                    }
                }
                if (_Content is string)
                {
                    xUIText xUIText = new xUIText();
                    xUIText.Content = _Content;
                    RealContent = xUIText;
                }
            }
        }
        public Site13Event OnClickEvent => ClickEvent;
    }
}
