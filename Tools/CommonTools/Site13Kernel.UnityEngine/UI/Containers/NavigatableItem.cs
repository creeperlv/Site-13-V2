using Site13Kernel.Core;
using Site13Kernel.UI.Elements;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.UI.Containers
{
    [Serializable]
    public class NavigatableItem:ControlledBehavior
    {
        public List<GenericInteractivable> AccessPoints;
        public GameObject ActuallControlledItem;
        public bool _isShown = true;
        public virtual void Show(float DeltaT)
        {
            if (!_isShown)
            {
                ActuallControlledItem.SetActive(true);
                _isShown = true;
            }
        }
        public virtual void Hide(float DeltaT)
        {
            if (_isShown)
            {
                ActuallControlledItem.SetActive(false);
                _isShown = false;
            }
        }
    }
}
