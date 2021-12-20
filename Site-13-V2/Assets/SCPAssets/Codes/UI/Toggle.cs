using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Site13Kernel.UI
{
    /// <summary>
    /// Modification 
    /// </remarks>
    [AddComponentMenu("UI/Toggle", 30)]
    [RequireComponent(typeof(RectTransform))]
    public class Toggle : Selectable, IPointerClickHandler, ISubmitHandler, ICanvasElement, IEditable
    {
        /// <summary>
        /// Display settings for when a toggle is activated or deactivated.
        /// </summary>
        public enum ToggleTransition
        {
            /// <summary>
            /// Show / hide the toggle instantly
            /// </summary>
            None,

            /// <summary>
            /// Fade the toggle in / out smoothly.
            /// </summary>
            Fade
        }

        [Serializable]
        /// <summary>
        /// UnityEvent callback for when a toggle is toggled.
        /// </summary>
        public class ToggleEvent : UnityEvent<bool>
        { }

        /// <summary>
        /// Transition mode for the toggle.
        /// </summary>
        public ToggleTransition toggleTransition = ToggleTransition.Fade;

        /// <summary>
        /// Graphic the toggle should be working with.
        /// </summary>
        public Graphic graphic;


        public ToggleEvent onValueChanged = new ToggleEvent();

        // Whether the toggle is on
        [Tooltip("Is the toggle currently on or off?")]
        [SerializeField]
        private bool m_IsOn;

        protected Toggle()
        { }

        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

        [Serializable]
        public class ButtonClickedEvent : UnityEvent
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
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) && !Application.isPlaying)
                CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }

#endif // if UNITY_EDITOR

        public virtual void Rebuild(CanvasUpdate executing)
        {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
            {
                if (!isCallbackLocked)
                {
                    onValueChanged.Invoke(m_IsOn);
                    if (OnValueChanged != null)
                    {
                        OnValueChanged(m_IsOn);
                    }
                }
            }
#endif
        }

        public virtual void LayoutComplete()
        { }

        public virtual void GraphicUpdateComplete()
        { }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            PlayEffect(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnDidApplyAnimationProperties()
        {
            // Check if isOn has been changed by the animation.
            // Unfortunately there is no way to check if we don�t have a graphic.
            if (graphic != null)
            {
                bool oldValue = !Mathf.Approximately(graphic.canvasRenderer.GetColor().a, 0);
                if (m_IsOn != oldValue)
                {
                    m_IsOn = oldValue;
                    Set(!oldValue);
                }
            }

            base.OnDidApplyAnimationProperties();
        }


        public bool isOn
        {
            get { return m_IsOn; }

            set
            {
                Set(value);
            }
        }

        /// <summary>
        /// Set isOn without invoking onValueChanged callback.
        /// </summary>
        /// <param name="value">New Value for isOn.</param>
        public void SetIsOnWithoutNotify(bool value)
        {
            Set(value, false);
        }

        void Set(bool value, bool sendCallback = true)
        {
            if (m_IsOn == value)
                return;

            m_IsOn = value;

            PlayEffect(toggleTransition == ToggleTransition.None);
            if (sendCallback)
            {
                UISystemProfilerApi.AddMarker("Toggle.value", this);
                if (!isCallbackLocked)
                {
                    onValueChanged.Invoke(m_IsOn); 
                    m_OnClick.Invoke();
                    if (OnValueChanged != null)
                    {
                        OnValueChanged(m_IsOn);
                    }
                }
            }
        }
        bool isCallbackLocked = false;
        private void PlayEffect(bool instant)
        {
            if (graphic == null)
                return;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                graphic.canvasRenderer.SetAlpha(m_IsOn ? 1f : 0f);
            else
#endif
                graphic.CrossFadeAlpha(m_IsOn ? 1f : 0f, instant ? 0f : 0.1f, true);
        }

        /// <summary>
        /// Assume the correct visual state.
        /// </summary>
        protected override void Start()
        {
            PlayEffect(true);
        }

        private void InternalToggle()
        {
            if (!IsActive() || !IsInteractable())
                return;

            isOn = !isOn;
        }

        /// <summary>
        /// React to clicks.
        /// </summary>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            InternalToggle();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            InternalToggle();
        }
        Action<object> OnValueChanged = null;
        public void SetCallback(Action<object> callback)
        {
            OnValueChanged = callback;
        }

        public void SetValue(object obj)
        {
            isOn = (bool)obj;
        }

        public void InitValue(object obj)
        {
            isCallbackLocked = true;
            try
            {
                isOn = (bool)obj;
            }
            catch (Exception)
            {
            }
            isCallbackLocked = false;
        }

        public object GetValue()
        {
            return isOn;
        }
    }
}
