using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI
{
    public class ToggleButton : UIButton
    {
        [Header("Normal Color")]
        public ColorBlock NormalColor;
        [Header("Selected Color")]
        public ColorBlock SelectedColor;
        public Action<bool> OnToggle = null;
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
                        OnToggle(value);
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
    }
}
