using xUI.Core.UIElements;

namespace xUI.Core.Abstraction
{
    public interface IBackground
    {
        UIElement Background { get; set; }
        void SetIBackgroundImpl(IBackgroundImpl impl);
    }
    public interface IBackgroundImpl
    {
        void SetBackground(UIElement Background);
    }
}
