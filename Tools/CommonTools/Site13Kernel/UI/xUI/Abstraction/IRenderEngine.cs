namespace Site13Kernel.UI.xUI.Abstraction
{
    public interface IRenderEngine
    {
        void CommitUITree(IUIElement element);
        void AttachUITree(IUIElement Parent, IUIElement Children);
        void RemoveUITree(IUIElement element);
    }
}
