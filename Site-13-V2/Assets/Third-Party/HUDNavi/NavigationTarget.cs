using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HUDNavi
{
    public class NavigationTarget : MonoBehaviour
    {
        [HideInInspector]
        public NavigationPoint MappedHUDPoint = null;
        [HideInInspector]
        public NavigationPoint MappedHUDArrow = null;
        [HideInInspector]
        public bool isAdded = false;
        public bool Show = true;
        public bool _Show = true;
        public string label;
        public bool ShowDistance;
        public string TargetNavigationPointType;
        public bool WillShowOffScreenPoint = false;
        [Header("This will override MaxPointDistance, -2 means always show, -1 means no modify.")]
        public float OverrideMaxShowDistance = -1;
        [HideInInspector]
        public Vector3 SafePos;
        [HideInInspector]
        public bool SafeMappedHUDPointActive = false;
        [HideInInspector]
        public bool SafeMappedHUDArrowActive = false;
        [HideInInspector]
        public bool HierarchyActive = true;
        // Update is called once per frame
        void Update()
        {
            if (NavigationCore.CurrentCore == null)
            {
                isAdded = false;
            }
            if (isAdded == false)
            {
                if (NavigationCore.CurrentCore != null)
                {
                    //lock (NavigationCore.CurrentCore.Targets)
                    {
                        NavigationCore.CurrentCore.Init(this);

                    }
                }
            }
            _Show = Show;
            if (MappedHUDPoint != null)
                SafeMappedHUDPointActive = MappedHUDPoint.gameObject.activeSelf;
            if (MappedHUDArrow != null)
                SafeMappedHUDArrowActive = MappedHUDArrow.gameObject.activeSelf;
            SafePos = transform.position;
        }
        private void OnEnable()
        {
            HierarchyActive = true;
        }
        private void OnDisable()
        {
            _Show = false;
            HierarchyActive = false;
        }
        private void OnDestroy()
        {
            if (NavigationCore.CurrentCore != null)
                NavigationCore.CurrentCore.Targets.Remove(this);
            if (MappedHUDArrow != null) GameObject.Destroy(MappedHUDArrow.gameObject);
            if (MappedHUDPoint != null) GameObject.Destroy(MappedHUDPoint.gameObject);
        }
        public void UpdateLabel(string NewLabel)
        {
            label = NewLabel;
            if (MappedHUDPoint != null)
                MappedHUDPoint.Label.text = label;
            if (MappedHUDArrow != null) MappedHUDArrow.Label.text = label;
        }
    }

}