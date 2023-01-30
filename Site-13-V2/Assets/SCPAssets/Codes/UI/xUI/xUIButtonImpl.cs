using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using xUI.Core.Abstraction;

namespace Site13Kernel.UI.xUI
{
    public class xUIButtonImpl : MonoBehaviour, IClickableImplementation
    {
        public Button button;
        private void Start()
        {
            button.onClick.AddListener(OnClick);
        }
        public void OnClick()
        {
            if (onClick != null)
            {
                onClick();
            }
        }
        Action onClick = null;
        public void SetOnClick(Action onClick)
        {
            this.onClick = onClick;
        }
    }
}
