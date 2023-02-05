using LibCLCC.NET.Delegates;
using System;
using System.Diagnostics;
using System.Numerics;
using xUI.Core.Abstraction;
using xUI.Core.Attributes;
using xUI.Core.Helpers;

namespace xUI.Core.UIElements
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
        bool _Resizable = true;
        public bool Resizable
        {
            get => _Resizable; set
            {
                _Resizable = value;
            }
        }
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
        xUIAlignment _VerticalAlignment = xUIAlignment.Center;
        public xUIAlignment VerticalAlignment { get => _VerticalAlignment; [TODO] set => _VerticalAlignment = value; }
        xUIAlignment _HorizontalAlignment = xUIAlignment.Center;
        public xUIAlignment HorizontalAlignment
        {
            get => _HorizontalAlignment;
            [TODO]
            set => _HorizontalAlignment = value;
        }
        WindowMode _WindowMode = WindowMode.Full;
        public WindowMode WindowMode
        {
            get => _WindowMode;
            set
            {
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
        BreakableFunc<Vector2> _OnResize = new BreakableFunc<Vector2>();
        public BreakableFunc<Vector2> OnResize => _OnResize;

        BreakableFunc _OnClose = new BreakableFunc();
        public BreakableFunc OnClose => _OnClose;

        public IMenu MenuBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ShowBackButton { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ExtendContentToTitleBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public BreakableFunc BackEvent => throw new NotImplementedException();

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
                    {
                        if (value is string s)
                        {
                            Title = s;
                        }
                    }
                    break;
                case "HorizontalAlignment":
                    {
                        if (value is string s)
                            this.LayoutAlignment(s, false);
                    }
                    break;
                case "VerticalAlignment":
                    {
                        if (value is string s)
                            this.LayoutAlignment(s, true);
                    }
                    break;
                case "Size":
                    {
                        if (value is string s)
                            this.Size(s);
                    }
                    break;
                case "Width":
                    {

                    }
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
            AbstractRenderEngine.CurrentEngine.RemoveUITree(this);
        }

        public void Show()
        {
            wimpl.Show();
        }

        public void Hide()
        {
            wimpl.Hide();
        }
        IxUILayoutableImpl ixUILayoutableImpl = null;
        public void SetIxUILayoutableImpl(IxUILayoutableImpl impl)
        {
            if (impl != null)
                return;
            ixUILayoutableImpl = impl;
        }
        IContentImpl contentImpl = null;
        public void SetIContentImpl(IContentImpl impl)
        {
            if (contentImpl != null)
                return;
            contentImpl = impl;
        }

        public void Back()
        {
            throw new NotImplementedException();
        }
    }
}
