using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI
{
    public class ToggleButton : UIButton
    {
        public ColorBlock NormalColor;
        public ColorBlock SelectedColor;
        public Action<bool> OnToggle=null;
        public bool _IsOn=false;
        public bool IsOn
        {
            get
            {
                return _IsOn;
            }
            set
            {
                _IsOn = value;
                if (OnToggle != null)
                    OnToggle(value);
                this.colors = value ? SelectedColor : NormalColor;
            }
        }
        protected override void Start()
        {
            OnClick = () =>
            {
                IsOn = !_IsOn;
            };
        }
        protected override void Awake()
        {
            OnClick = () =>
            {
                IsOn = !_IsOn;
            };

            this.colors = _IsOn ? SelectedColor : NormalColor;
        }
    }
}
