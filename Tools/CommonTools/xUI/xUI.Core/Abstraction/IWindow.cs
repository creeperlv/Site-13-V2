using Site13Kernel.Core;
using System.Numerics;

namespace xUI.Core.Abstraction
{
    public interface IWindow : IFocusable, IContent
    {
        string Title { get; set; }
        bool Resizable { get; set; }
        bool ShowBackButton { get; set; }
        bool ExtendContentToTitleBar { get; set; }
        BreakableEvent BackEvent { get; }
        WindowMode WindowMode { get; set; }
        BreakableEvent<Vector2> OnResize { get; }
        BreakableEvent OnClose { get; }
        IMenu MenuBar { get; set; }
        void SetIWindowImpl(IWindowImpl impl);
        IWindowImpl GetIWindowImpl();
        void SetTitle(string title);
        void SetIcon(object obj);
        void Close();
        void Show();
        void Hide();
        void Back();
    }
    public interface IWindowImpl
    {
        void SetExtendContentToTitleBar(bool value);
        void SetBackButton(bool Show);
        void SetTitle(string title);
        void SetIcon(object obj);
        void SetMenuBar(IMenu menu);
        void SetWindowMode(WindowMode wm);
        void Close();
        void Show();
        void Hide();
        void DisableDefaultWindowBackground();
    }
}
