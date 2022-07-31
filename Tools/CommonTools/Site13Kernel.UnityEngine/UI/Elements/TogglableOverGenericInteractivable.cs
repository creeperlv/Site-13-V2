using Site13Kernel.Core;
using UnityEngine;

namespace Site13Kernel.UI.Elements
{
    public class TogglableOverGenericInteractivable : GenericInteractivable
    {
        private bool _isChecked;
        public bool IsChecked { get => _isChecked; set { _isChecked = value; } }
        public bool AllowUncheckByClick = true;
        public Site13Event Checked = new Site13Event();
        public Site13Event Unchecked = new Site13Event();
        public GameObject MarkGroup;
        public Animator Animator;
        public string CheckedTrigger;
        public string UncheckedTrigger;
        internal override void InvokeOnClick()
        {
            if (_isChecked)
            {
                if (AllowUncheckByClick)
                {
                    IsChecked = false;
                    Unchecked.Invoke();
                    ApplyVisual();
                }
            }
            else
            {
                IsChecked = true;
                ApplyVisual();
                Checked.Invoke();
            }
            base.InvokeOnClick();
        }
        public override void SideInit()
        {
            ApplyVisual();
        }
        public void ApplyVisual()
        {
            if (Animator != null)
            {
                Animator.SetTrigger(_isChecked ? CheckedTrigger : UncheckedTrigger);
            }
            else
            {
                MarkGroup.SetActive(_isChecked);
            }
        }
    }
}
