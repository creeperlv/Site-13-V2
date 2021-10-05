using Site13Kernel.Core;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField]
        float minValue;
        [SerializeField]
        float maxValue;
        public int textDisplayPrecision;
        public int TextDisplayPrecision
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => textDisplayPrecision;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (textDisplayPrecision != value)
                {
                    OnValueUpdate();
                }
                textDisplayPrecision = value;
            }
        }
        public bool OverrideTextColor;
        public List<SegmentedColor> MainColor;
        public List<SegmentedColor> TextColor;
        public float MinValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => minValue;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (minValue != value)
                    OnValueUpdate();
                minValue = value;
            }
        }
        public float MaxValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => maxValue;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (maxValue != value)
                    OnValueUpdate();
                maxValue = value;
            }
        }
        public Image FillImage;
        public Text ProgressText;
        public bool isTextEnabled;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnValueUpdate()
        {
            var _V = Mathf.InverseLerp(MinValue, MaxValue, V);
            var _V100 = _V * 100;
            FillImage.fillAmount = _V;
            if (MainColor != null)
            {
                if (MainColor.Count > 0)
                {
                    var C = FindColor(_V, MainColor);
                    FillImage.color = C;
                }
            }
            if (OverrideTextColor)
            {
                if (TextColor != null)
                {
                    if (TextColor.Count > 0)
                    {
                        var C = FindColor(_V, TextColor);
                        ProgressText.color = C;
                    }
                }
            }

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color FindColor(float V, List<SegmentedColor> segmenteds)
        {
            Color? L = MainColor[0].TargetColor;
            Color? N = null;
            float LB = 0;
            float UB = -1;
            foreach (var item in MainColor)
            {
                if (V < item.UpperBound)
                {
                    if (N.HasValue)
                        L = N;
                    if (UB != -1)
                        LB = UB;
                    N = item.TargetColor;
                    UB = item.UpperBound;
                }
            }
            if (L.HasValue && N.HasValue)
            {
                if (UB != -1)
                {
                    return MathUtilities.Lerp(L.Value, N.Value, Mathf.InverseLerp(LB, UB, V));
                }
            }
            return Color.white;
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
                    return maxValue;
                case "MinValue":
                    return minValue;
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
