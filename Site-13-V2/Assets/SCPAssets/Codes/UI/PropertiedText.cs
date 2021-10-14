
using UnityEngine;
using Site13Kernel.Core;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

namespace Site13Kernel.UI
{
    public class PropertiedText : PropertiedControlledBehavior
    {
        public Text UnityUI_Text;
        public TMPro.TMP_Text TMP_Text;
        string _Content=null;
        public bool _Visibility=false;
        public bool Visibility
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _Visibility;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value != _Visibility)
                {
                    SetVisibility(value);
                    _Visibility = value;
                }
            }
        }
        public string Content
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _Content;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value != _Content)
                {
                    SetText(value);
                    _Content = value;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVisibility(bool Visibility)
        {

            if (UnityUI_Text != null)
            {
                UnityUI_Text.gameObject.SetActive(Visibility);
            }
            if (TMP_Text != null)
            {
                TMP_Text.gameObject.SetActive(Visibility);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetText(string Content)
        {
            if (UnityUI_Text != null)
            {
                UnityUI_Text.text = Content;
            }
            if (TMP_Text != null)
            {
                TMP_Text.text = Content;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void SetProperty(string name, object value)
        {
            switch (name)
            {
                case "Content":
                    Content = (string)value;
                    break;
                default:
                    break;
            }
        }
        public override object GetPropertyValue(string name)
        {
            switch (name)
            {
                case "Content":
                    return Content;
                default:
                    break;
            }
            return null;
        }
    }
}
