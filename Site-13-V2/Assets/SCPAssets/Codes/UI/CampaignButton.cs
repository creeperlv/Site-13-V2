using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Site13Kernel.GameLogic;
using Site13Kernel.Data;
using CLUNL.Localization;
using Site13Kernel.Core;

namespace Site13Kernel.UI
{
    public class CampaignButton : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, IVisualElement
    {
        public GameObject HintImage;
        public Image BackgroundImage;
        public Text MissionName;
        public Action OnClick = null;
        [HideInInspector]
        public CampaignButtonGroup CampaignParent;
        protected CampaignButton()
        {
        }
        [Serializable]
        public class ButtonClickedEvent : UnityEvent
        {
        }
        // Event delegates triggered on click.
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
        internal MissionDefinition definition;
        public void Init(CampaignButtonGroup parent, MissionDefinition definition)
        {
            CampaignParent = parent;
            parent.Children.Add(this);
            this.definition = definition;
            try
            {
                BackgroundImage.sprite = GameRuntime.CurrentGlobals.CurrentGameDef.Sprites[this.definition.ImageName].LoadedSprite;

            }
            catch (Exception)
            {
            }
            MissionName.text = Language.Find(this.definition.NameID, this.definition.DispFallback);
        }
        public void Hint()
        {
            if (HintImage != null) HintImage.SetActive(true);
        }
        public void Unhint()
        {
            if (HintImage != null) HintImage.SetActive(false);
        }
        private void Press()
        {
            if (IsActive() && IsInteractable())
            {
                if (CampaignParent != null)
                {
                    CampaignParent.UnHintAll();
                    CampaignParent.Selected = this;
                }
                this.Hint();
                m_OnClick.Invoke();
                if (OnClick != null)
                    OnClick();
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Press();
            }
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();
            if (IsActive() && IsInteractable())
            {
                DoStateTransition(SelectionState.Pressed, instant: false);
                StartCoroutine(OnFinishSubmit());
            }
        }

        private IEnumerator OnFinishSubmit()
        {
            float fadeTime = base.colors.fadeDuration;
            float elapsedTime = 0f;
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(base.currentSelectionState, instant: false);
        }

        public void SetProperty(string Key, object Value)
        {
            throw new NotImplementedException();
        }

        public void SetProperty(string Key, string Value)
        {
            throw new NotImplementedException();
        }

        public Property GetProperty(string Key)
        {
            throw new NotImplementedException();
        }

        public object GetPropertyValue(string Key)
        {
            throw new NotImplementedException();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
    [Serializable]
    public class CampaignButtonGroup
    {
        public List<CampaignButton> Children = new List<CampaignButton>();
        public CampaignButton Selected;
        public void UnHintAll()
        {
            foreach (var item in Children)
            {
                item.Unhint();
            }
        }
    }
}
