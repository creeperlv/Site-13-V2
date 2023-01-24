using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Site13Kernel
{
    public class xUIDraggable : MonoBehaviour, IDragHandler,IBeginDragHandler
    {
        public Transform MovingPart;
        Vector2 Delta=Vector3.zero;
        public void OnBeginDrag(PointerEventData eventData)
        {
            Delta=new Vector2(MovingPart.position.x, MovingPart.position.y) -eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            MovingPart.position= eventData.position+Delta;
        }
    }
}
