using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Site13Kernel.UI.BlamUI
{

    public class BlamButton : BlamControl
    {
        public Image TargetGraphic;
        public Color Normal;
        public Color Selected;
        public Color Pressed;
        public Color Disabled;
        public Site13Event OnClick = new Site13Event();
        public override void ActivateAction()
        {
            OnClick.Invoke();
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            TargetGraphic.color = Pressed;
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            TargetGraphic.color = isSelected ? Selected : Normal;
        }
        public override void UnSelect()
        {
            TargetGraphic.color = Normal;
        }
        public override void Select()
        {
            TargetGraphic.color= Selected;
        }
    }
}
