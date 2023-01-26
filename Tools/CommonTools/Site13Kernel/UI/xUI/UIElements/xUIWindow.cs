using Site13Kernel.UI.xUI.Abstraction;
using System;

namespace Site13Kernel.UI.xUI.UIElements
{
    public class xUIWindow : UIElement, IWindow, IxUILayoutable
    {
        public string _Title;
        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
                if (IsInitialized())
                    wimpl.SetTitle(value);
            }
        }
        public bool Resizable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        object _content = null;
        UIElement _content_g = null;
        public object Content
        {
            get => _content;
            set
            {
                if (IsInitialized())
                {
                    if (value == null)
                    {
                        if (_content_g != null)
                        {
                            AbstractRenderEngine.CurrentEngine.RemoveUITree(_content_g);
                        }
                    }
                }
                _content = value;
            }
        }
        public xUIAlignment VerticalAlignment { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public xUIAlignment HorizontalAlignment { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        WindowMode _WindowMode = WindowMode.Full;
        public WindowMode WindowMode
        {
            get => _WindowMode;
            set {
                if (IsInitialized())
                {
                    if (_WindowMode != value)
                    {
                        _WindowMode = value;
                        wimpl.SetWindowMode(value);
                    }
                }
                _WindowMode = value;
            }
        }

        public void Focus()
        {
            IWindowManager.CurrentManager.BringWindowToFront(this);
        }

        public void GainFocus()
        {
            IWindowManager.CurrentManager.BringWindowToFront(this);
        }

        public void LostFocus()
        {
        }

        public void SetIcon(object obj)
        {
            wimpl.SetIcon(obj);
        }
        IWindowImpl wimpl = null;
        public void SetIWindowImpl(IWindowImpl impl)
        {
            if (wimpl != null) return;
            wimpl = impl;
        }

        public override void SetProperty(string name, object value)
        {
            switch (name)
            {
                case "Title":
                    if (value is string s)
                    {
                        Title = s;
                    }
                    break;
                case "HorizontalAlignment":
                    break;
                default:
                    break;
            }
            base.SetProperty(name, value);
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void Unfocus()
        {
            throw new NotImplementedException();
        }

        public IWindowImpl GetIWindowImpl()
        {
            return wimpl;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Show()
        {
            throw new NotImplementedException();
        }

        public void Hide()
        {
            throw new NotImplementedException();
        }

        public void SetIxUILayoutableImpl(IxUILayoutableImpl impl)
        {
            throw new NotImplementedException();
        }
    }
}
