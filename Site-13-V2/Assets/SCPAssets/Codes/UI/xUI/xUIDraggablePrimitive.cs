using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Site13Kernel
{
    public class xUIDraggablePrimitive : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        public Action<Vector2> OnDragDelta = null;
        Vector2 StartPoint;
        public void OnBeginDrag(PointerEventData eventData)
        {
            StartPoint = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (OnDragDelta != null)
                OnDragDelta(StartPoint-eventData.position );
            StartPoint= eventData.position;
        }
    }
}
