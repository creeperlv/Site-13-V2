using Site13Kernel.Core;
using Site13Kernel.GameLogic.Cameras;
using Site13Kernel.UI.Elements;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.UI.Containers
{
    [Serializable]
    public class NavigatableItem : ControlledBehavior
    {
        public BasicNavigationContainer ParentContainer;
        public List<GenericInteractivable> AccessPoints;
        public GameObject ActuallControlledItem;
        public CamPosTargetBase TargetCamPos;
        public bool isSmooth;
        public bool _isShown = true;
        public void OnShown()
        {
            if (ParentContainer != null)
            {
                var willSmooth = ParentContainer.Children[ParentContainer.LastSelectedIndex].isSmooth && isSmooth;
                if (willSmooth)
                {
                    if (ImmersiveCamBase.GlobalCam != null)
                        ImmersiveCamBase.GlobalCam.SmoothFollow = willSmooth;
                }
                ParentContainer.UpdateLastIndex();
            }
            if (TargetCamPos!=null)
                TargetCamPos.SoftTakeControl();

        }
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
