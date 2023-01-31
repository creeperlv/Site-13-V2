using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using xUI.Core.Abstraction;

namespace xUI.Core.UIElements
{
    public class UIElement : IUIElement, ISize, IPosition
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
        Vector2 _Size;
        public Vector2 Size
        {
            get => _Size;
            set
            {
                SetSize(value);
            }
        }

        bool _inited = false;
        public virtual void Initialize()
        {
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

        public virtual void SetPosition(Vector2 Position)
        {
        }
        internal ISizeImplementation SizeImplementation;
        public virtual void SetISizeImplementation(ISizeImplementation implementation)
        {
            SizeImplementation = implementation;
        }

        public virtual void SetSize(Vector2 Size)
        {
            if (IsInitialized())
            {
                if (_Size != Size)
                {
                    SizeImplementation.SetSize(Size);
                    _Size = Size;
                }
            }
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

        public virtual void SetPositionDataOnly(Vector2 Position)
        {

        }

        public virtual void SetSizeDataOnly(Vector2 Size)
        {
            this._Size = Size;
        }

        public void SetHitEnabledDataOnly(bool hitEnabled)
        {
            ElementImplementation.SetIsEnable(hitEnabled);
        }
    }
}
