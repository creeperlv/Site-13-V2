namespace xUI.Core.Abstraction
{
    public interface IxUITextable
    {
        int FontSize { get; set; }
        string FontFamily { get; set; }
        void SetIxUITextableImpl(IxUITextableImpl impl);
    }
    public interface IxUITextableImpl
    {
        void SetFontSize(int s);
        void SetFontFamily(string s);
    }
}
