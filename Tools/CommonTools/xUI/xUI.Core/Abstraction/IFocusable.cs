using LibCLCC.NET.Delegates;
using xUI.Core.Events;

namespace xUI.Core.Abstraction
{
    public interface IFocusable
    {
        BreakableFunc<FocusEvent> OnGainFocus { get;  }
        BreakableFunc<FocusEvent> OnLostFocus { get;  }
        void Focus();
        void Unfocus();
        void LostFocus();
        void GainFocus();
        void SetIFocusableImpl(IFocusableImpl impl);
    }
    public interface IFocusableImpl
    {
        void Focus();
    }
}
