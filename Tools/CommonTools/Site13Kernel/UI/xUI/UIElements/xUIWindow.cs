using Site13Kernel.Attributes;
using Site13Kernel.UI.xUI.Abstraction;
using Site13Kernel.UI.xUI.Helpers;
using System;
using System.Diagnostics;

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
#if DEBUG
                Trace.WriteLine("xUIWindow set content to:"+value.GetType().Name);
#endif
                _content = value;
            }
        }
        xUIAlignment _VerticalAlignment = xUIAlignment.Center;
        public xUIAlignment VerticalAlignment { get => _VerticalAlignment; [TODO] set => _VerticalAlignment=value; }
        xUIAlignment _HorizontalAlignment= xUIAlignment.Center;
        public xUIAlignment HorizontalAlignment { get => _HorizontalAlignment; 
            [TODO]set => _HorizontalAlignment=value; }
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
                            PropertyHelper.LayoutAlignment(this, s, false);
                    }
                    break;
                case "VerticalAlignment":
                    {
                        if (value is string s)
                            PropertyHelper.LayoutAlignment(this, s, true);
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
            this.wimpl.Show();
        }

        public void Hide()
        {
            this.wimpl.Hide();
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
    }
}
