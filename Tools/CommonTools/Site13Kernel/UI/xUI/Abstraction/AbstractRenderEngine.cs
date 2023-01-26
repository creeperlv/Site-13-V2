namespace Site13Kernel.UI.xUI.Abstraction
{
    public abstract class AbstractRenderEngine : IRenderEngine
    {
        public static AbstractRenderEngine CurrentEngine = null;
        public abstract void AttachUITree(IUIElement Parent, IUIElement Children);
        public abstract void CommitUITree(IUIElement element);
        public abstract void RemoveUITree(IUIElement element);
    }
    public interface IWindowManager
    {
        public static IWindowManager CurrentManager = null;
        void BringWindowToFront(IWindow window);
        void MakeTopMostWindow(IWindow window);
    }
}
