using Site13Kernel.Core;
using Site13Kernel.UI.xUI.UIElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Site13Kernel.UI.xUI.Abstraction
{
    public interface IUIElement
    {
        List<IUIElement> Children { get; set; }
        UIElement Parent { get; set; }
        string Name { get; set; }
        bool IsEnabled { get; set; }
        bool IsHitEnabled { get; set; }
        void SetProperty(string name, object value);
        object GetProperty(string name);
        void SetHitEnabledDataOnly(bool hitEnabled);
        void SetIUIElementImplementation(IUIElementImplementation implementation);
        void Initialize();
        bool IsInitialized();
    }
    public interface IContent
    {
        object Content { get; set; }
        void SetIContentImpl(IContentImpl impl);
    }
    public interface IContentImpl { 
        void SetContent(object content);
    }
    public interface IxUIContainer
    {
        void Add(object content);
        void Remove(object content);
    }
    public interface IxUILayoutable
    {
        void SetIxUILayoutableImpl(IxUILayoutableImpl impl);
        xUIAlignment VerticalAlignment { get; set; }
        xUIAlignment HorizontalAlignment { get; set; }
    }
    public interface IxUILayoutableImpl
    {
        void SetVerticalAlignment(xUIAlignment alignment);
        void SetHorizontalAlignment(xUIAlignment alignment);
    }
    public interface IWindow : IFocusable, IContent
    {
        string Title { get; set; }
        bool Resizable { get; set; }
        WindowMode WindowMode { get; set; }
        void SetIWindowImpl(IWindowImpl impl);
        IWindowImpl GetIWindowImpl();
        void SetTitle(string title);
        void SetIcon(object obj);
        void Close();
        void Show();
        void Hide();
    }
    public interface IWindowImpl
    {
        void SetTitle(string title);
        void SetIcon(object obj);
        void SetWindowMode(WindowMode wm);
        void Close();
        void Show();
        void Hide();
    }
    public interface IFocusable
    {
        void Focus();
        void Unfocus();
        void LostFocus();
        void GainFocus();
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
        Vector2 Size { get; set; }
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
        void SetIsEnable(bool State);
        void SetHit(bool IsEnabled);
    }
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
