using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Site13Kernel.UI.BlamUI
{
    public class BlamControl : BlamBase, IPointerEnterHandler, ISelectHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public bool isSelected;
        protected override void OnEnable()
        {
            BlamUICore.Core.RegisterOrEnable(this);
            OnEnableSide();
        }
        public virtual void OnEnableSide() { }
        public virtual void OnDisableSide() { }
        protected override void OnDisable()
        {
            BlamUICore.Core.Disable(this);
            OnDisableSide();
        }
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (isEnabled)
                BlamUICore.Core.Select(this);
        }

        public virtual void ActivateAction()
        {

        }
        public virtual void UnSelect()
        {
            isSelected = false;
        }
        public virtual void Select()
        {
            isSelected = true;
        }
        public virtual void OnSelect(BaseEventData eventData)
        {
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (isEnabled)
                BlamUICore.Core.Activate(this);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {

        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }
    }
}
