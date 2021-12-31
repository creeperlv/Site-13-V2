using Site13Kernel.Core;
using Site13Kernel.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;
using UnityEngine.UI.CoroutineTween;

namespace Site13Kernel.UI
{
    public partial class ComboBox : IEditable, IPropertiedObject, IVisualElement
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetValue()
        {
            return this.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InitValue(object obj)
        {
            value = (int)obj;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCallback(Action<object> callback)
        {
            this.callback = callback;
        }
        Action<object> callback=null;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Callback()
        {
            if(callback!= null)
            callback(this.value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(object obj)
        {
            value = (int)obj;
            Callback();
        }
        public Visibility Visibility
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _visibility;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {

                _visibility = value;
                switch (_visibility)
                {
                    case UI.Visibility.Visible:
                        Show();
                        break;
                    case UI.Visibility.Hidden:
                        Hide();
                        break;
                    case UI.Visibility.Collapsed:
                        Collapse();
                        break;
                    default:
                        break;
                }
            }
        }
        private Visibility _visibility;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetProperty(string name, object value)
        {
            switch (name)
            {
                case "Selection":
                    if (value is int)
                    {
                        SetValue(value);
                    }
                    else if (value is string)
                    {
                        SetValue(int.Parse((string)value));
                    }
                    break;
                case "Visibility":
                    if (value is Visibility v)
                    {
                        Visibility = v;
                    }
                    break;
                default:
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetProperty(Property p)
        {
            SetProperty(p.Key, p.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Property GetProperty(string name)
        {
            var v = GetPropertyValue(name);
            if (v != null) return new Property { Key = name, Value = v };
            else return null;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public object GetPropertyValue(string name)
        {
            switch (name)
            {
                case "Selection":
                    return value;
                case "Visibility":
                    {
                        return Visibility;
                    }
                default:
                    break;
            }
            return null;
        }

        public void Collapse()
        {
            _visibility= Visibility.Collapsed;
        }

        public void Size()
        {
        }
    }
}
