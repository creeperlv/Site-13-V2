using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Site13Kernel.UI
{
    public interface IVisualElement
    {
        void Show();
        void Hide();
    }
    public interface IPropertiedObject
    {
        void SetProperty(string name, object value);
        void SetProperty(Property p);
        Property GetProperty(string name);
        object GetPropertyValue(string name);
    }
}
