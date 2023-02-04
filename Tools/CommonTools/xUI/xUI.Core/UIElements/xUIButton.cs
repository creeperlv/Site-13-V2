using LibCLCC.NET.Delegates;
using System;
using xUI.Core.Abstraction;
using xUI.Core.Data;

namespace xUI.Core.UIElements
{
    public class xUIButton : UIElement, IClickable, IContent, IxUIPadding
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

        ChainAction ClickEvent = new ChainAction();
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
        IxUIPaddingImpl paddingImpl = null;
        public void SetIPaddingImpl(IxUIPaddingImpl impl)
        {
            if (paddingImpl != null)
            {
                return;
            }
            paddingImpl = impl;
        }

        public ChainAction OnClickEvent => ClickEvent;
        xUIThickness _Padding = null;
        public xUIThickness Padding
        {
            get => _Padding; set
            {
                if (IsInitialized())
                {

                }
                _Padding = value;
            }
        }
    }
}
