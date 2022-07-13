using CLUNL.Localization;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Site13Kernel.UI.Elements
{
    [Serializable]
    [AddComponentMenu("UI/Site13/TextButton")]
    public class TextButton : Button {
        [SerializeField]
        public Text ControlledText;
        public bool InitEffect;
        public PrefabReference Effect;
        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            onClick.Invoke();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            if (InitEffect)
            {
                try
                {
                    EffectController.CurrentEffectController.Spawn(Effect, Vector3.zero, Quaternion.identity, false);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            Press();
        }
        public void SetText(LocalizedString Content)
        {
            if (ControlledText != null) ControlledText.text = Content;
        }
    }
}
