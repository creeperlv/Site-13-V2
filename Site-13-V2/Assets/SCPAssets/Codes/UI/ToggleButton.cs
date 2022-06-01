using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI
{
    public class ToggleButton : UIButton,IEditable
    {
        [Header("Normal Color")]
        public ColorBlock NormalColor;
        [Header("Selected Color")]
        public ColorBlock SelectedColor;
        public List<Action<bool>> OnToggle = new List<Action<bool>>();
        public bool _IsOn = false;
        public bool PreventClickToOff = false;
        [HideInInspector]
        public bool SuspendCallback = false;
        public bool IsOn
        {
            get
            {
                return _IsOn;
            }
            set
            {
                _IsOn = value;
                if (!SuspendCallback)
                    if (OnToggle != null)
                        foreach (var item in OnToggle)
                        {
                            item(value);
                        }
                this.colors = value ? SelectedColor : NormalColor;
            }
        }
        public void SetValue(bool value)
        {
            _IsOn = value;
            this.colors = value ? SelectedColor : NormalColor;
        }
        protected override void Start()
        {
            OnClick = () =>
            {
                if (PreventClickToOff)
                {
                    IsOn = true;

                }
                else
                    IsOn = !_IsOn;
            };
        }
        protected override void Awake()
        {
            OnClick = () =>
            {
                if (PreventClickToOff)
                {
                    IsOn = true;

                }
                else
                    IsOn = !_IsOn;
            };

            this.colors = _IsOn ? SelectedColor : NormalColor;
        }

        public void SetCallback(Action<object> callback)
        {
            OnToggle.Add((v) => { callback(v); });
        }

        public void SetValue(object obj)
        {
            if(obj is bool b)
            {
                IsOn = b;
            }
        }

        public void InitValue(object obj)
        {
            SuspendCallback = true;
            if (obj is bool b)
            {
                IsOn = b;
            }
            SuspendCallback = false;
        }

        public object GetValue()
        {
            return IsOn;
        }
    }
}
