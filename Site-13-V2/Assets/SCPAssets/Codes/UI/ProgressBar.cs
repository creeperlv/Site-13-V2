using Site13Kernel.Core;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                {
                    V = value;
                    OnValueUpdate();
                }
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
                    textDisplayPrecision = value;
                    OnValueUpdate();
                }
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
                {
                    minValue = value;
                    OnValueUpdate();

                }
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
                {
                    maxValue = value;
                    OnValueUpdate();
                }
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
            Color? N = MainColor.Last().TargetColor;
            float LB = MainColor[0].UpperBound;
            float UB = MainColor[0].UpperBound;
            {
                SegmentedColor C = MainColor.Last();
                foreach (var item in MainColor)
                {
                    if (item.UpperBound < C.UpperBound)
                    {
                        if (V < item.UpperBound)
                        {
                            C = item;
                        }
                    }

                }
                N = C.TargetColor;
                UB = C.UpperBound;
            }
            {
                SegmentedColor C = MainColor[0];
                foreach (var item in MainColor)
                {
                    if (item.UpperBound > C.UpperBound)
                    {
                        if (V > item.UpperBound)
                        {
                            C = item;
                        }
                    }

                }
                L = C.TargetColor;
                LB = C.UpperBound;
            }
            if (L.HasValue && N.HasValue)
            {
                if (UB != -1)
                {
                    return MathUtilities.Lerp(L.Value, N.Value, Mathf.InverseLerp(LB, UB, V));
                }
            }
            {
                var __L = MainColor.Last();
                if (V >= __L.UpperBound) return __L.TargetColor;
            }
            if (L.HasValue)
            {
                return L.Value;
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
