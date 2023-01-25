using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Site13Kernel
{
    public class xUIPointerDownImpl : MonoBehaviour,IPointerDownHandler
    {
        public Action onPointerDown = null;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (onPointerDown != null)
            {
                onPointerDown();
            }

        }
    }
}
