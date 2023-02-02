using xUI.Core.Data;

namespace xUI.Core.Abstraction
{
    public interface IxUIPadding
    {
        xUIThickness Padding { get; set; }
        void SetIPaddingImpl(IxUIPaddingImpl impl);
    }
    public interface IxUIPaddingImpl
    {
        void SetPadding(xUIThickness padding);
    }
}
