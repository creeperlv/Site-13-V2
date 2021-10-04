using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI
{
    public class ProgressBar : ControlledBehavior, IPropertiedObject, IContentable
    {
        float V;
        public float Value
        {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => V;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (V != value)
                    OnValueUpdate();
                V = value;
            }
        }
        float MinV;
        float MaxV;
        public float MinValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MinV;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (MinV != value)
                    OnValueUpdate();
                MinV = value;
            }
        }
        public float MaxValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MaxV;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (MaxV != value)
                    OnValueUpdate();
                MaxV = value;
            }
        }
        public Image FillImage;
        public Text ProgressText;
        public bool isTextEnabled;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnValueUpdate()
        {

            FillImage.fillAmount = Mathf.InverseLerp(MinValue, MaxValue, V);
        }
        public string Content
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Value.ToString();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Value = float.Parse(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Property GetProperty(string name)
        {
            var v = GetPropertyValue(name);
            if (v == null)
                return null;
            else
                return new Property { Key = name, Value = v };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetPropertyValue(string name)
        {
            switch (name)
            {
                case "Value":
                    return Value;
                case "MaxValue":
                    return MaxV;
                case "MinValue":
                    return MinV;
                default:
                    break;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetProperty(string name, object value)
        {
            switch (name)
            {
                case "Value":
                    Value = (float)value;
                    break;
                case "MaxValue":
                    MaxValue = (float)value;
                    break;
                case "MinValue":
                    MinValue = (float)value;
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
    }
}
