using LibCLCC.NET.Delegates;
using System;
using xUI.Core.Abstraction;

namespace xUI.Core.UIElements
{
    public class xUIMenu : UIElement, IMenu
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
    public class xUIMenuItem : UIElement, IMenuItem
    {
        public object Content { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ChainAction OnClickEvent => throw new NotImplementedException();

        public void Add(object content)
        {
            throw new NotImplementedException();
        }

        public void OnClick()
        {
            throw new NotImplementedException();
        }

        public void Remove(object content)
        {
            throw new NotImplementedException();
        }

        public void SetIClickableImplementation(IClickableImplementation implementation)
        {
            throw new NotImplementedException();
        }

        public void SetIContentImpl(IContentImpl impl)
        {
            throw new NotImplementedException();
        }
    }
}
