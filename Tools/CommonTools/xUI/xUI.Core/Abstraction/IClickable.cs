using Site13Kernel.Core;
using System;

namespace xUI.Core.Abstraction
{
    public interface IClickable
    {
        void SetIClickableImplementation(IClickableImplementation implementation);
        Site13Event OnClickEvent { get; }
        void OnClick();
    }
    public interface IClickableImplementation
    {
        void SetOnClick(Action onClick);
        void OnClick();
    }
}
