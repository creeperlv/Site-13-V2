using Site13Kernel.UI.xUI.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Site13Kernel.UI.xUI
{
    public class xUIClickablePrimitiveImpl : MonoBehaviour, IPointerClickHandler, IClickableImplementation
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
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
