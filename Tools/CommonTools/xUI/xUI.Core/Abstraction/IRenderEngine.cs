namespace xUI.Core.Abstraction
{
    public interface IRenderEngine
    {
        void CommitUITree(IUIElement element);
        void AttachUITree(IUIElement Parent, IUIElement Children);
        void RemoveUITree(IUIElement element);
    }
}
