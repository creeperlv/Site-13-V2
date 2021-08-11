using Site13Kernel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Site13Kernel.UI
{
    /// <summary>
    /// A standard button that sends an event when clicked.
    /// </summary>
    [AddComponentMenu("Site13/UI/Button", 30)]
    public class UIButton : Selectable, IPointerClickHandler, ISubmitHandler, IVisualElement, IPropertiedObject, IContentable
    {

        public Text PrimaryContentText;
        public string Content
        {
            get => PrimaryContentText.text;
            set => PrimaryContentText.text = value;
        }

        public Action OnClick=null;

        [Serializable]
        public class ButtonClickedEvent : UnityEvent
        {
        }

        // Event delegates triggered on click.
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

        protected UIButton()
        {
        }

        public ButtonClickedEvent onClick
        {
            get
            {
                return m_OnClick;
            }
            set
            {
                m_OnClick = value;
            }
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            m_OnClick.Invoke();
            if (OnClick != null)
            {
                OnClick();
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }


        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();

            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }
        public bool Visibility
        {
            get => this.gameObject.activeSelf;
            set => this.gameObject.SetActive(value);
        }
        public void Show()
        {
            Visibility = true;
        }

        public void Hide()
        {
            Visibility = false;
        }

        public void SetProperty(string name, object value)
        {
            switch (name)
            {
                case "Visibility":
                    Visibility = (bool) value;
                    break;
                case "Content":
                    Content = (string) value;
                    break;
                default:
                    break;
            }
        }

        public void SetProperty(Property p)
        {
            SetProperty(p.Key, p.Value);
        }

        public Property GetProperty(string name)
        {
            Property property=new Property();
            property.Key = name;
            property.Value = GetPropertyValue(name);
            if (property.Value == null)
                return null;
            else
                return property;
        }

        public object GetPropertyValue(string name)
        {
            switch (name)
            {
                case "Visibility":
                    return Visibility;
                case "Content":
                    return Content;
                default:
                    break;
            }
            return null;
        }
    }
}
