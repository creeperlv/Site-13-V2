using Site13Kernel.Core;
using System;
using System.Runtime.CompilerServices;

namespace Site13Kernel.UI
{
    //Extension part of TextBox.
    public partial class TextBox : IEditable, IVisualElement, IPropertiedObject
    {
        public object GetValue()
        {
            return text;
        }

        public void InitValue(object obj)
        {
            text = obj.ToString();
        }
        Action<object> callback;
        public void SetCallback(Action<object> callback)
        {
            this.callback = callback;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Callback()
        {
            callback(text);
        }
        public void SetValue(object obj)
        {
            text = obj.ToString();
            Callback();
        }
        public void Show()
        {
            this.gameObject.SetActive(true);
        }
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
        public void InitProperty(Property p)
        {
            InitProperty(p.Key, p.Value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InitProperty(string name, object value)
        {

            switch (name)
            {
                case "Value":
                    SetValue(value);
                    break;
                case "Visibility":
                    gameObject.SetActive((bool)value);
                    break;
                default:
                    break;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetProperty(string name, object value)
        {
            switch (name)
            {
                case "Value":
                    SetValue(value);
                    break;
                case "Visibility":
                    if ((bool)value) Show();
                    else Hide();
                    break;
                default:
                    break;
            }
        }

        public void SetProperty(Property p)
        {
            SetProperty(p.Key, p.Value);
        }

        public Property GetProperty(string name)
        {
            var V = GetPropertyValue(name);
            return V == null ? null : new Property { Value = V, Key = name };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetPropertyValue(string name)
        {
            switch (name)
            {
                case "Value":
                    return GetValue();
                case "Visibility":
                    return gameObject.activeSelf;
                default:
                    break;
            }
            return null;
        }
    }
}
