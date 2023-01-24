using Site13Kernel.Core;
using Site13Kernel.UI.xUI.UIElements;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Site13Kernel.UI.xUI.Abstraction
{
    public interface IUIElement
    {
        List<UIElement> Children { get; set; }
        UIElement Parent { get; set; }
        string Name { get; set; }
        void SetProperty(string name, object value);
        object GetProperty(string name);
        void SetIUIElementImplementation(IUIElementImplementation implementation);
        void Initialize();
    }
    public interface IClickable
    {
        void SetIClickableImplementation(IClickableImplementation implementation);
        void AddEvent(Site13Event actions);
        void OnClick();
    }
    public interface IClickableImplementation
    {
        void SetOnClick(Action onClick);
        void OnClick();
    }
    public interface IPosition
    {
        void SetIPositionImplementation(IPositionImplementation implementation);
        void SetPosition(Vector2 Position);
        void SetPositionDataOnly(Vector2 Position);
    }
    public interface IPositionImplementation
    {
        void SetPosition(Vector2 Position);
    }
    public interface ISize
    {
        void SetISizeImplementation(ISizeImplementation implementation);
        void SetSize(Vector2 Size);
        void SetSizeDataOnly(Vector2 Size);
    }
    public interface ISizeImplementation
    {
        void SetSize(Vector2 Size);
    }
    public interface IUIElementImplementation
    {
        void Repaint();
    }
    public interface IRenderEngine
    {
        void CommitUITree(IUIElement element);

    }
    public abstract class AbstractRenderEngine : IRenderEngine
    {
        public abstract void CommitUITree(IUIElement element);
    }
}
