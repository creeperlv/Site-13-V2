using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        public bool InitEffect;
        public PrefabReference Effect;
        public string Content
        {
            get => PrimaryContentText.text;
            set => PrimaryContentText.text = value;
        }

        public Action OnClick = null;

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
            if (InitEffect)
            {
                try
                {

                    GameRuntime.CurrentGlobals.CurrentEffectController.Spawn(Effect, Vector3.zero, Quaternion.identity, false);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
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
        public Visibility Visibility
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _visibility;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {

                _visibility = value;
                switch (_visibility)
                {
                    case UI.Visibility.Visible:
                        Show();
                        break;
                    case UI.Visibility.Hidden:
                        Hide();
                        break;
                    case UI.Visibility.Collapsed:
                        Collapse();
                        break;
                    default:
                        break;
                }
            }
        }
        private Visibility _visibility;

        public void Show()
        {
            this.gameObject.SetActive(true);
            _visibility = Visibility.Visible;
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
            _visibility = Visibility.Hidden;
        }

        public void SetProperty(string name, object value)
        {
            switch (name)
            {
                case "Visibility":
                    {
                        //Visibility = Enum.Parse(typeof(UI.Visibility), value);
                        Visibility = (Visibility)value;
                    }
                    break;
                case "Content":
                    Content = (string)value;
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
            Property property = new Property();
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

        public void Collapse()
        {
            _visibility = Visibility.Collapsed;
        }

        public void Size()
        {
        }
    }
}
