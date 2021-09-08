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
    public partial class ComboBox :
        IEditable, IPropertiedObject, IVisualElement
    {
        public object GetValue()
        {
            return this.value;
        }

        public void InitValue(object obj)
        {
            value = (int)obj;
        }

        public void SetCallback(Action<object> callback)
        {
            this.callback = callback;
        }
        Action<object> callback;
        void Callback()
        {
            callback(this.value);
        }
        public void SetValue(object obj)
        {
            value = (int)obj;
            Callback();
        }
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
                    if (value is bool b)
                    {
                        if (b)
                        {
                            Show();
                        }
                        else
                        {
                            Hide();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void SetProperty(Property p)
        {
            throw new NotImplementedException();
        }

        public Property GetProperty(string name)
        {
            throw new NotImplementedException();
        }

        public object GetPropertyValue(string name)
        {
            throw new NotImplementedException();
        }
    }
}
