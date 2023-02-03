using Site13Kernel.UI.xUI.Utilities;
using Site13Kernel.UI.xUI.uUIImplementation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using xUI.Core.Abstraction;
using xUI.Core.Data;
using xUI.Core.UIElements;

namespace Site13Kernel.UI.xUI
{
    public class xUIButtonImpl : xUIElementImplBase, IClickableImplementation,IxUIPaddingImpl,IContentImpl,ISizeImplementation
    {
        public RectTransform OuterTransform;
        public RectTransform Content;
        public Button button;
        public xUIThickness Padding;
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
        Vector2 oldSize;
        private void Update()
        {
            if (oldSize != Content.sizeDelta)
            {
                Repaint();
            }
        }
        public void Repaint()
        {
            this.OuterTransform.sizeDelta = Content.sizeDelta;

            oldSize = Content.sizeDelta;

        }

        public void SetIsEnable(bool State)
        {
            this.button.gameObject.SetActive(State);
        }

        public void SetHit(bool IsEnabled)
        {
            this.button.image.raycastTarget = IsEnabled;
        }

        public void SetPadding(xUIThickness padding)
        {
            this.Padding = padding;
        }

        public void Bind(IUIElement element)
        {
            if(element is xUIButton btn)
            {
                btn.SetIContentImpl(this);
                btn.SetIPaddingImpl(this);
                btn.SetIClickableImplementation(this);
                btn.SetISizeImplementation(this);
                btn.SetIUIElementImplementation(this);
            }
        }

        public void SetContent(object content)
        {
            if(content is IUIElement element)
            {

            }
            Repaint();
        }

        public void SetSize(System.Numerics.Vector2 Size)
        {
            oldSize = Size.ToUnityVector();
            Repaint();
        }
    }
}
