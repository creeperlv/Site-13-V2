using Site13Kernel.Core;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Site13Kernel.UI.Elements
{

    [Serializable]
    [AddComponentMenu("UI/Site13/ToggleButton")]
    public class ToggleButton : TextButton
    {
        [FormerlySerializedAs("AnimatedMark")]
        [SerializeField]
        public Animator AnimatedMark;
        [FormerlySerializedAs("CheckedTrigger")]
        [SerializeField]
        public string CheckedTrigger;
        [FormerlySerializedAs("UnCheckedTrigger")]
        [SerializeField]
        public string UnCheckedTrigger;
        [FormerlySerializedAs("CheckedMouseOnTrigger")]
        [SerializeField]
        public string CheckedMouseOnTrigger;
        [FormerlySerializedAs("UnCheckedMouseOnTrigger")]
        [SerializeField]
        public string UnCheckedMouseOnTrigger;
        public Site13Event Checked;
        public Site13Event Unchecked;
        bool _isOn = false;
        [FormerlySerializedAs("PreventUncheckOnClick")]
        [SerializeField]
        public bool PreventUncheckOnClick = false;
        bool _isPointerIn = false;
        [SerializeField]
        public bool isOn
        {
            get => _isOn; set
            {
                _isOn = value;
                ApplyState();
                if (_isOn) Checked.Invoke();
                else Unchecked.Invoke();
            }
        }
        public new void OnEnable()
        {
            base.OnEnable();
            ApplyState();
        }
        void ApplyState()
        {
            if (_isPointerIn)
            {
                AnimatedMark.SetTrigger(_isOn ? CheckedMouseOnTrigger : UnCheckedMouseOnTrigger);
            }
            else
            AnimatedMark.SetTrigger(_isOn ? CheckedTrigger : UnCheckedTrigger);
        }
        protected ToggleButton()
        {
            onClick.AddListener(() =>
            {
                if (PreventUncheckOnClick && _isOn) return;
                isOn = !_isOn;
            });
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            AnimatedMark.SetTrigger(_isOn ? CheckedMouseOnTrigger : UnCheckedMouseOnTrigger);
            _isPointerIn = true;
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            AnimatedMark.SetTrigger(_isOn ? CheckedTrigger : UnCheckedTrigger);
            _isPointerIn = false;
        }
    }
}
