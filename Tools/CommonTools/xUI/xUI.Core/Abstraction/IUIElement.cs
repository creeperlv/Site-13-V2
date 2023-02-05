using LibCLCC.NET.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using xUI.Core.Data;
using xUI.Core.UIElements;

namespace xUI.Core.Abstraction
{
    public interface IUIElement
    {
        List<IUIElement> Children { get; set; }
        [JsonIgnore]
        UIElement Parent { get; set; }
        string Name { get; set; }
        bool IsEnabled { get; set; }
        bool IsHitEnabled { get; set; }
        string Variant { get; set; }
        Style StyleResources { get; set; }
        ReactableList<string> Styles { get; }
        void SetProperty(string name, object value);
        object GetProperty(string name);
        void SetHitEnabledDataOnly(bool hitEnabled);
        void SetIUIElementImplementation(IUIElementImplementation implementation);
        void Initialize();
        bool IsInitialized();
    }
    public interface IUIElementImplementation
    {
        void SetStyles(List<string> styles);
        void Repaint();
        void SetIsEnable(bool State);
        void SetHit(bool IsEnabled);
        void Bind(IUIElement element);
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

    public interface IFocusable
    {
        void Focus();
        void Unfocus();
        void LostFocus();
        void GainFocus();
    }
}
