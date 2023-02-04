using LibCLCC.NET.Delegates;
using System;

namespace xUI.Core.Abstraction
{
    public interface IClickable
    {
        void SetIClickableImplementation(IClickableImplementation implementation);
        ChainAction OnClickEvent { get; }
        void OnClick();
    }
    public interface IClickableImplementation
    {
        void SetOnClick(Action onClick);
        void OnClick();
    }
}
