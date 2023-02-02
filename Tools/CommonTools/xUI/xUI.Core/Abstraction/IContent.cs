namespace xUI.Core.Abstraction
{
    public interface IContent
    {
        object Content { get; set; }
        void SetIContentImpl(IContentImpl impl);
    }
    public interface IContentImpl
    {
        void SetContent(object content);
    }
}
