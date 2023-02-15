using LibCLCC.NET.Collections;
using LibCLCC.NET.Delegates;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using xUI.Core.Abstraction;
using xUI.Core.Data;
using xUI.Core.Events;
using xUI.Core.Helpers;

namespace xUI.Core.UIElements
{
    public class UIElement : IUIElement, ISize, IPosition, IFocusable
    {
        List<IUIElement> _Children = new List<IUIElement>();
        public virtual List<IUIElement> Children { get => _Children; set => _Children = value; }
        [JsonIgnore]
        public UIElement Parent { get; set; }
        public virtual string Name { get; set; }
        bool _isEnabled;
        bool _isHitEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                ElementImplementation.SetIsEnable(value);
            }
        }
        public bool IsHitEnabled
        {
            get => _isHitEnabled;
            set
            {
                _isHitEnabled = value;
                ElementImplementation.SetHit(value);
            }
        }
        Vector2 _Size = new Vector2(float.NaN, float.NaN);
        public Vector2 Size
        {
            get => _Size;
            set
            {
                SetSize(value);
            }
        }
        ReactableList<string> _Styles = new ReactableList<string>();
        public bool StyleReact(ReactableList<string> NewL, string newS)
        {
            return false;
        }
        public ReactableList<string> Styles => _Styles;
        string _Variant = null;
        public string Variant { get => _Variant; set => _Variant = value; }
        Style _Style = null;
        public Style StyleResources { get => _Style; set => _Style = value; }
        public Vector2 Position { get => _Pos; set => SetPosition(value); }
        BreakableFunc<FocusEvent> _OnGainFocus = new BreakableFunc<FocusEvent>();
        public BreakableFunc<FocusEvent> OnGainFocus => _OnGainFocus;

        BreakableFunc<FocusEvent> _OnLostFocus = new BreakableFunc<FocusEvent>();
        public BreakableFunc<FocusEvent> OnLostFocus => _OnLostFocus;

        bool _inited = false;
        public virtual void Initialize()
        {
            foreach (var item in Styles)
            {
                this.ApplyStyle(item);
            }
            _Styles.ReactChain.Add(StyleReact);
            _inited = true;
        }

        public bool IsInitialized()
        {
            return _inited;
        }

        internal IUIElementImplementation ElementImplementation;
        public void SetIUIElementImplementation(IUIElementImplementation implementation)
        {
            ElementImplementation = implementation;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SetPosition(Vector2 Position)
        {
            if (_Pos == Position) return;
            _Pos = Position;
            if (IsInitialized())
                positionImplementation.SetPosition(Position);
        }
        internal ISizeImplementation SizeImplementation;
        public virtual void SetISizeImplementation(ISizeImplementation implementation)
        {
            SizeImplementation = implementation;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SetSize(Vector2 Size)
        {
            if (_Size == Size)
                return;
            if (IsInitialized())
            {
                SizeImplementation.SetSize(Size);
            }
            _Size = Size;
        }

        public virtual void SetProperty(string name, object value)
        {
            switch (name)
            {
                case "Name":
                    {
                        this.Name = value as string ?? "";
                    }
                    break;
                case "Variant":
                    {
                        Variant = value as string;
                    }
                    break;
                case "Styles":
                    {
                        var strs = value as string;
                        if (strs != null)
                        {
                            strs = strs.Trim();
                            var _s = strs.Split(',', ' ');
                            foreach (var item in _s)
                            {
                                Styles.Add(item);
                            }
                        }
                    }
                    break;
                case "Size":
                    {
                        var s = value as string;
                        if (s != null)
                            this.Size(s);
                    }
                    break;
                default:
                    break;
            }
        }

        public virtual object GetProperty(string name)
        {
            switch (name)
            {
                case "Name":
                    return this.Name;
                case "Variant":
                    return Variant;
                case "Styles":
                    return Styles;
                default:
                    break;
            }
            return null;
        }
        internal IPositionImplementation positionImplementation = null;
        public virtual void SetIPositionImplementation(IPositionImplementation implementation)
        {
            if (positionImplementation != null) return;
            positionImplementation = implementation;
        }
        Vector2 _Pos;
        public virtual void SetPositionDataOnly(Vector2 Position)
        {
            _Pos = Position;
        }

        public virtual void SetSizeDataOnly(Vector2 Size)
        {
            this._Size = Size;
        }

        public void SetHitEnabledDataOnly(bool hitEnabled)
        {
            ElementImplementation.SetIsEnable(hitEnabled);
        }

        public virtual void Focus()
        {
            GainFocus();
        }

        public virtual void Unfocus()
        {
            LostFocus();
        }

        public virtual void LostFocus()
        {
            if (Parent != null)
                Parent.LostFocus();
        }

        public virtual void GainFocus()
        {
            if (Parent != null)
                Parent.GainFocus();
        }
        IFocusableImpl focusableImpl = null;
        public void SetIFocusableImpl(IFocusableImpl impl)
        {
            if (focusableImpl != null) return;
            focusableImpl = impl;
        }
        Vector2 DesiredSize=new Vector2(float.NaN,float.NaN);
        public void SetDesireSize(Vector2 Size)
        {
            DesiredSize = Size;
        }
    }
}
