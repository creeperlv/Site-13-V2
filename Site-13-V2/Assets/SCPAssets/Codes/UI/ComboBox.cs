using Site13Kernel.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;
using UnityEngine.UI.CoroutineTween;

namespace Site13Kernel.UI
{
    /// <summary>
    /// Heavily modified from original UnityEngine.UI.Dropdown.
    /// </summary>
    [AddComponentMenu("UI/UIDropdown", 35)]
    [RequireComponent(typeof(RectTransform))]
    public partial class ComboBox : Selectable, IPointerClickHandler, ISubmitHandler, ICancelHandler
    {
        [SerializeField]
        private RectTransform m_Template;
        public RectTransform template { get { return m_Template; } set { m_Template = value; RefreshShownValue(); } }
        [SerializeField]
        private Text m_CaptionText;
        public Text captionText { get { return m_CaptionText; } set { m_CaptionText = value; RefreshShownValue(); } }

        [SerializeField]
        private Image m_CaptionImage;
        public Image captionImage { get { return m_CaptionImage; } set { m_CaptionImage = value; RefreshShownValue(); } }

        [Space]

        [SerializeField]
        private Text m_ItemText;
        public Text itemText { get { return m_ItemText; } set { m_ItemText = value; RefreshShownValue(); } }

        [SerializeField]
        private Image m_ItemImage;
        public Image itemImage { get { return m_ItemImage; } set { m_ItemImage = value; RefreshShownValue(); } }

        [Space]

        [SerializeField]
        private int m_Value;

        [Space]
        [SerializeField]
        private ComboBoxDataList m_Options = new ComboBoxDataList();
        public List<ComboBoxData> options
        {
            get { return m_Options.options; }
            set { m_Options.options = value; RefreshShownValue(); }
        }

        [Space]
        [SerializeField]
        private ComboBoxEvent m_OnValueChanged = new ComboBoxEvent();
        public ComboBoxEvent onValueChanged { get { return m_OnValueChanged; } set { m_OnValueChanged = value; } }

        [SerializeField]
        private float m_AlphaFadeSpeed = 0.15f;

        public float alphaFadeSpeed { get { return m_AlphaFadeSpeed; } set { m_AlphaFadeSpeed = value; } }

        private GameObject m_Dropdown;
        private GameObject m_Blocker;
        private List<ComboBoxItem> m_Items = new List<ComboBoxItem>();
        private TweenRunner<FloatTween> m_AlphaTweenRunner;
        private bool validTemplate = false;
        private const int kHighSortingLayer = 30000;

        private static ComboBoxData s_NoOptionData = new ComboBoxData();

        public int value
        {
            get
            {
                return m_Value;
            }
            set
            {
                Set(value);
            }
        }
        public void SetValueWithoutNotify(int input)
        {
            Set(input, false);
        }

        void Set(int value, bool sendCallback = true)
        {
            if (Application.isPlaying && (value == m_Value || options.Count == 0))
                return;

            m_Value = Mathf.Clamp(value, 0, options.Count - 1);
            RefreshShownValue();

            if (sendCallback)
            {
                UISystemProfilerApi.AddMarker("Dropdown.value", this);
                m_OnValueChanged.Invoke(m_Value);
                Callback();
            }
        }

        protected ComboBox()
        { }

        protected override void Awake()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            if (m_CaptionImage)
                m_CaptionImage.enabled = (m_CaptionImage.sprite != null);

            if (m_Template)
                m_Template.gameObject.SetActive(false);
        }

        protected override void Start()
        {
            m_AlphaTweenRunner = new TweenRunner<FloatTween>();
            m_AlphaTweenRunner.Init(this);
            base.Start();

            RefreshShownValue();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!IsActive())
                return;

            RefreshShownValue();
        }

#endif
        protected override void OnDisable()
        {
            //Destroy dropdown and blocker in case user deactivates the dropdown when they click an option (case 935649)
            ImmediateDestroyDropdownList();

            if (m_Blocker != null)
                DestroyBlocker(m_Blocker);
            m_Blocker = null;

            base.OnDisable();
        }

        public void RefreshShownValue()
        {
            ComboBoxData data = s_NoOptionData;

            if (options.Count > 0)
                data = options[Mathf.Clamp(m_Value, 0, options.Count - 1)];

            if (m_CaptionText)
            {
                if (data != null && data.text != null)
                    m_CaptionText.text = data.text;
                else
                    m_CaptionText.text = "";
            }

            if (m_CaptionImage)
            {
                if (data != null)
                    m_CaptionImage.sprite = data.image;
                else
                    m_CaptionImage.sprite = null;
                m_CaptionImage.enabled = (m_CaptionImage.sprite != null);
            }
        }

        public void AddOptions(List<ComboBoxData> options)
        {
            this.options.AddRange(options);
            RefreshShownValue();
        }

        public void AddOptions(List<string> options)
        {
            var optionsCount = options.Count;
            for (int i = 0; i < optionsCount; i++)
                this.options.Add(new ComboBoxData(options[i]));
            RefreshShownValue();
        }
        public void AddOptions(List<Sprite> options)
        {
            var optionsCount = options.Count;
            for (int i = 0; i < optionsCount; i++)
                this.options.Add(new ComboBoxData(options[i]));
            RefreshShownValue();
        }

        public void ClearOptions()
        {
            options.Clear();
            m_Value = 0;
            RefreshShownValue();
        }

        private void SetupTemplate(Canvas rootCanvas)
        {
            validTemplate = false;

            if (!m_Template)
            {
                Debugger.CurrentDebugger.Log("The dropdown template is not assigned. The template needs to be assigned and must have a child GameObject with a Toggle component serving as the item.", LogLevel.Error);
                return;
            }

            GameObject templateGo = m_Template.gameObject;
            templateGo.SetActive(true);
            UnityEngine.UI.Toggle itemToggle = m_Template.GetComponentInChildren<UnityEngine.UI.Toggle>();

            validTemplate = true;
            if (itemToggle == null || itemToggle.transform == template)
            {
                validTemplate = false;
                Debugger.CurrentDebugger.LogError("The dropdown template is not valid. The template must have a child GameObject with a Toggle component serving as the item.", template);
            }
            else if (!(itemToggle.transform.parent is RectTransform))
            {
                validTemplate = false;
                Debugger.CurrentDebugger.LogError("The dropdown template is not valid. The child GameObject with a Toggle component (the item) must have a RectTransform on its parent.", template);
            }
            else if (itemText != null && !itemText.transform.IsChildOf(itemToggle.transform))
            {
                validTemplate = false;
                Debugger.CurrentDebugger.LogError("The dropdown template is not valid. The Item Text must be on the item GameObject or children of it.", template);
            }
            else if (itemImage != null && !itemImage.transform.IsChildOf(itemToggle.transform))
            {
                validTemplate = false;
                Debugger.CurrentDebugger.LogError("The dropdown template is not valid. The Item Image must be on the item GameObject or children of it.", template);
            }

            if (!validTemplate)
            {
                templateGo.SetActive(false);
                return;
            }

            ComboBoxItem item = itemToggle.gameObject.AddComponent<ComboBoxItem>();
            item.text = m_ItemText;
            item.image = m_ItemImage;
            item.toggle = itemToggle;
            item.rectTransform = (RectTransform)itemToggle.transform;

            // Find the Canvas that this dropdown is a part of
            Canvas parentCanvas = null;
            Transform parentTransform = m_Template.parent;
            while (parentTransform != null)
            {
                parentCanvas = parentTransform.GetComponent<Canvas>();
                if (parentCanvas != null)
                    break;

                parentTransform = parentTransform.parent;
            }

            // checks if a Canvas already exists before overriding it. (case 958281 - [UI] Child Canvas' Sorting Layer is changed to the same value as the parent)
            if (!templateGo.TryGetComponent<Canvas>(out _))
            {
                Canvas popupCanvas = templateGo.AddComponent<Canvas>();
                popupCanvas.overrideSorting = true;
                popupCanvas.sortingOrder = kHighSortingLayer;
                // popupCanvas used to assume the root canvas had the default sorting Layer, next line fixes (case 958281 - [UI] Dropdown list does not copy the parent canvas layer when the panel is opened)
                popupCanvas.sortingLayerID = rootCanvas.sortingLayerID;
            }

            // If we have a parent canvas, apply the same raycasters as the parent for consistency.
            if (parentCanvas != null)
            {
                Component[] components = parentCanvas.GetComponents<BaseRaycaster>();
                for (int i = 0; i < components.Length; i++)
                {
                    Type raycasterType = components[i].GetType();
                    if (templateGo.GetComponent(raycasterType) == null)
                    {
                        templateGo.AddComponent(raycasterType);
                    }
                }
            }
            else
            {
                GetOrAddComponent<GraphicRaycaster>(templateGo);
            }

            GetOrAddComponent<CanvasGroup>(templateGo);
            templateGo.SetActive(false);

            validTemplate = true;
        }

        private static T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (!comp)
                comp = go.AddComponent<T>();
            return comp;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Show();
        }
        public virtual void OnSubmit(BaseEventData eventData)
        {
            Show();
        }

        public virtual void OnCancel(BaseEventData eventData)
        {
            Hide();
        }

        public void Show()
        {
            if (!IsActive() || !IsInteractable() || m_Dropdown != null)
                return;

            _visibility = Visibility.Visible;
            // Get root Canvas.
            var list = ListPool<Canvas>.Get();
            gameObject.GetComponentsInParent(false, list);
            if (list.Count == 0)
                return;

            // case 1064466 rootCanvas should be last element returned by GetComponentsInParent()
            var listCount = list.Count;
            Canvas rootCanvas = list[listCount - 1];
            for (int i = 0; i < listCount; i++)
            {
                if (list[i].isRootCanvas || list[i].overrideSorting)
                {
                    rootCanvas = list[i];
                    break;
                }
            }

            ListPool<Canvas>.Release(list);

            if (!validTemplate)
            {
                SetupTemplate(rootCanvas);
                if (!validTemplate)
                    return;
            }

            m_Template.gameObject.SetActive(true);


            // Instantiate the drop-down template
            m_Dropdown = CreateDropdownList(m_Template.gameObject);
            m_Dropdown.name = "Dropdown List";
            m_Dropdown.SetActive(true);

            // Make drop-down RectTransform have same values as original.
            RectTransform dropdownRectTransform = m_Dropdown.transform as RectTransform;
            dropdownRectTransform.SetParent(m_Template.transform.parent, false);

            // Instantiate the drop-down list items

            // Find the dropdown item and disable it.
            ComboBoxItem itemTemplate = m_Dropdown.GetComponentInChildren<ComboBoxItem>();

            GameObject content = itemTemplate.rectTransform.parent.gameObject;
            RectTransform contentRectTransform = content.transform as RectTransform;
            itemTemplate.rectTransform.gameObject.SetActive(true);

            // Get the rects of the dropdown and item
            Rect dropdownContentRect = contentRectTransform.rect;
            Rect itemTemplateRect = itemTemplate.rectTransform.rect;

            // Calculate the visual offset between the item's edges and the background's edges
            Vector2 offsetMin = itemTemplateRect.min - dropdownContentRect.min + (Vector2)itemTemplate.rectTransform.localPosition;
            Vector2 offsetMax = itemTemplateRect.max - dropdownContentRect.max + (Vector2)itemTemplate.rectTransform.localPosition;
            Vector2 itemSize = itemTemplateRect.size;

            m_Items.Clear();

            UnityEngine.UI.Toggle prev = null;
            var optionsCount = options.Count;
            for (int i = 0; i < optionsCount; ++i)
            {
                ComboBoxData data = options[i];
                ComboBoxItem item = AddItem(data, value == i, itemTemplate, m_Items);
                if (item == null)
                    continue;

                // Automatically set up a toggle state change listener
                item.toggle.isOn = value == i;
                item.toggle.onValueChanged.AddListener(x => OnSelectItem(item.toggle));

                // Select current option
                if (item.toggle.isOn)
                    item.toggle.Select();

                // Automatically set up explicit navigation
                if (prev != null)
                {
                    Navigation prevNav = prev.navigation;
                    Navigation toggleNav = item.toggle.navigation;
                    prevNav.mode = Navigation.Mode.Explicit;
                    toggleNav.mode = Navigation.Mode.Explicit;

                    prevNav.selectOnDown = item.toggle;
                    prevNav.selectOnRight = item.toggle;
                    toggleNav.selectOnLeft = prev;
                    toggleNav.selectOnUp = prev;

                    prev.navigation = prevNav;
                    item.toggle.navigation = toggleNav;
                }
                prev = item.toggle;
            }

            // Reposition all items now that all of them have been added
            Vector2 sizeDelta = contentRectTransform.sizeDelta;
            sizeDelta.y = itemSize.y * (m_Items.Count+1) + offsetMin.y - offsetMax.y;
            contentRectTransform.sizeDelta = sizeDelta;

            float extraSpace = dropdownRectTransform.rect.height - contentRectTransform.rect.height;
            if (extraSpace > 0)
                dropdownRectTransform.sizeDelta = new Vector2(dropdownRectTransform.sizeDelta.x, dropdownRectTransform.sizeDelta.y+ itemSize.y - extraSpace);

            // Invert anchoring and position if dropdown is partially or fully outside of canvas rect.
            // Typically this will have the effect of placing the dropdown above the button instead of below,
            // but it works as inversion regardless of initial setup.
            Vector3[] corners = new Vector3[4];
            dropdownRectTransform.GetWorldCorners(corners);

            RectTransform rootCanvasRectTransform = rootCanvas.transform as RectTransform;
            Rect rootCanvasRect = rootCanvasRectTransform.rect;
            for (int axis = 0; axis < 2; axis++)
            {
                bool outside = false;
                for (int i = 0; i < 4; i++)
                {
                    Vector3 corner = rootCanvasRectTransform.InverseTransformPoint(corners[i]);
                    if ((corner[axis] < rootCanvasRect.min[axis] && !Mathf.Approximately(corner[axis], rootCanvasRect.min[axis])) ||
                        (corner[axis] > rootCanvasRect.max[axis] && !Mathf.Approximately(corner[axis], rootCanvasRect.max[axis])))
                    {
                        outside = true;
                        break;
                    }
                }
                if (outside)
                    RectTransformUtility.FlipLayoutOnAxis(dropdownRectTransform, axis, false, false);
            }

            var itemsCount = m_Items.Count;
            for (int i = 0; i < itemsCount; i++)
            {
                RectTransform itemRect = m_Items[i].rectTransform;
                itemRect.anchorMin = new Vector2(itemRect.anchorMin.x, 0);
                itemRect.anchorMax = new Vector2(itemRect.anchorMax.x, 0);
                itemRect.anchoredPosition = new Vector2(itemRect.anchoredPosition.x, offsetMin.y + itemSize.y * (itemsCount - 1 - i) + itemSize.y * itemRect.pivot.y);
                itemRect.sizeDelta = new Vector2(itemRect.sizeDelta.x, itemSize.y);
            }

            // Fade in the popup
            AlphaFadeList(m_AlphaFadeSpeed, 0f, 1f);

            // Make drop-down template and item template inactive
            m_Template.gameObject.SetActive(false);
            itemTemplate.gameObject.SetActive(false);

            m_Blocker = CreateBlocker(rootCanvas);
        }

        /// <summary>
        /// Create a blocker that blocks clicks to other controls while the dropdown list is open.
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to obtain a blocker GameObject.
        /// </remarks>
        /// <param name="rootCanvas">The root canvas the dropdown is under.</param>
        /// <returns>The created blocker object</returns>
        protected virtual GameObject CreateBlocker(Canvas rootCanvas)
        {
            // Create blocker GameObject.
            GameObject blocker = new GameObject("Blocker");

            // Setup blocker RectTransform to cover entire root canvas area.
            RectTransform blockerRect = blocker.AddComponent<RectTransform>();
            blockerRect.SetParent(rootCanvas.transform, false);
            blockerRect.anchorMin = Vector3.zero;
            blockerRect.anchorMax = Vector3.one;
            blockerRect.sizeDelta = Vector2.zero;

            // Make blocker be in separate canvas in same layer as dropdown and in layer just below it.
            Canvas blockerCanvas = blocker.AddComponent<Canvas>();
            blockerCanvas.overrideSorting = true;
            Canvas dropdownCanvas = m_Dropdown.GetComponent<Canvas>();
            blockerCanvas.sortingLayerID = dropdownCanvas.sortingLayerID;
            blockerCanvas.sortingOrder = dropdownCanvas.sortingOrder - 1;

            // Find the Canvas that this dropdown is a part of
            Canvas parentCanvas = null;
            Transform parentTransform = m_Template.parent;
            while (parentTransform != null)
            {
                parentCanvas = parentTransform.GetComponent<Canvas>();
                if (parentCanvas != null)
                    break;

                parentTransform = parentTransform.parent;
            }

            // If we have a parent canvas, apply the same raycasters as the parent for consistency.
            if (parentCanvas != null)
            {
                Component[] components = parentCanvas.GetComponents<BaseRaycaster>();
                for (int i = 0; i < components.Length; i++)
                {
                    Type raycasterType = components[i].GetType();
                    if (blocker.GetComponent(raycasterType) == null)
                    {
                        blocker.AddComponent(raycasterType);
                    }
                }
            }
            else
            {
                // Add raycaster since it's needed to block.
                GetOrAddComponent<GraphicRaycaster>(blocker);
            }


            // Add image since it's needed to block, but make it clear.
            Image blockerImage = blocker.AddComponent<Image>();
            blockerImage.color = Color.clear;

            // Add button since it's needed to block, and to close the dropdown when blocking area is clicked.
            Button blockerButton = blocker.AddComponent<Button>();
            blockerButton.onClick.AddListener(Hide);

            return blocker;
        }

        /// <summary>
        /// Convenience method to explicitly destroy the previously generated blocker object
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to dispose of a blocker GameObject that blocks clicks to other controls while the dropdown list is open.
        /// </remarks>
        /// <param name="blocker">The blocker object to destroy.</param>
        protected virtual void DestroyBlocker(GameObject blocker)
        {
            Destroy(blocker);
        }

        /// <summary>
        /// Create the dropdown list to be shown when the dropdown is clicked. The dropdown list should correspond to the provided template GameObject, equivalent to instantiating a copy of it.
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to obtain a dropdown list GameObject.
        /// </remarks>
        /// <param name="template">The template to create the dropdown list from.</param>
        /// <returns>The created drop down list gameobject.</returns>
        protected virtual GameObject CreateDropdownList(GameObject template)
        {
            return (GameObject)Instantiate(template);
        }

        /// <summary>
        /// Convenience method to explicitly destroy the previously generated dropdown list
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to dispose of a dropdown list GameObject.
        /// </remarks>
        /// <param name="dropdownList">The dropdown list GameObject to destroy</param>
        protected virtual void DestroyDropdownList(GameObject dropdownList)
        {
            Destroy(dropdownList);
        }

        /// <summary>
        /// Create a dropdown item based upon the item template.
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to obtain an option item.
        /// The option item should correspond to the provided template DropdownItem and its GameObject, equivalent to instantiating a copy of it.
        /// </remarks>
        /// <param name="itemTemplate">e template to create the option item from.</param>
        /// <returns>The created dropdown item component</returns>
        protected virtual ComboBoxItem CreateItem(ComboBoxItem itemTemplate)
        {
            return (ComboBoxItem)Instantiate(itemTemplate);
        }

        /// <summary>
        ///  Convenience method to explicitly destroy the previously generated Items.
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to dispose of an option item.
        /// Likely no action needed since destroying the dropdown list destroys all contained items as well.
        /// </remarks>
        /// <param name="item">The Item to destroy.</param>
        protected virtual void DestroyItem(ComboBoxItem item)
        { }

        // Add a new drop-down list item with the specified values.
        private ComboBoxItem AddItem(ComboBoxData data, bool selected, ComboBoxItem itemTemplate, List<ComboBoxItem> items)
        {
            // Add a new item to the dropdown.
            ComboBoxItem item = CreateItem(itemTemplate);
            item.rectTransform.SetParent(itemTemplate.rectTransform.parent, false);

            item.gameObject.SetActive(true);
            item.gameObject.name = "Item " + items.Count + (data.text != null ? ": " + data.text : "");

            if (item.toggle != null)
            {
                item.toggle.isOn = false;
            }

            // Set the item's data
            if (item.text)
                item.text.text = data.text;
            if (item.image)
            {
                item.image.sprite = data.image;
                item.image.enabled = (item.image.sprite != null);
            }

            items.Add(item);
            return item;
        }

        private void AlphaFadeList(float duration, float alpha)
        {
            CanvasGroup group = m_Dropdown.GetComponent<CanvasGroup>();
            AlphaFadeList(duration, group.alpha, alpha);
        }

        private void AlphaFadeList(float duration, float start, float end)
        {
            if (end.Equals(start))
                return;

            FloatTween tween = new FloatTween { duration = duration, startValue = start, targetValue = end };
            tween.AddOnChangedCallback(SetAlpha);
            tween.ignoreTimeScale = true;
            m_AlphaTweenRunner.StartTween(tween);
        }

        private void SetAlpha(float alpha)
        {
            if (!m_Dropdown)
                return;
            CanvasGroup group = m_Dropdown.GetComponent<CanvasGroup>();
            group.alpha = alpha;
        }

        public void Hide()
        {
            _visibility = Visibility.Hidden;
            if (m_Dropdown != null)
            {
                AlphaFadeList(m_AlphaFadeSpeed, 0f);

                // User could have disabled the dropdown during the OnValueChanged call.
                if (IsActive())
                    StartCoroutine(DelayedDestroyDropdownList(m_AlphaFadeSpeed));
            }
            if (m_Blocker != null)
                DestroyBlocker(m_Blocker);
            m_Blocker = null;
            Select();
        }

        private IEnumerator DelayedDestroyDropdownList(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            ImmediateDestroyDropdownList();
        }

        private void ImmediateDestroyDropdownList()
        {
            var itemsCount = m_Items.Count;
            for (int i = 0; i < itemsCount; i++)
            {
                if (m_Items[i] != null)
                    DestroyItem(m_Items[i]);
            }
            m_Items.Clear();
            if (m_Dropdown != null)
                DestroyDropdownList(m_Dropdown);
            m_Dropdown = null;
        }

        private void OnSelectItem(UnityEngine.UI.Toggle toggle)
        {
            if (!toggle.isOn)
                toggle.isOn = true;

            int selectedIndex = -1;
            Transform tr = toggle.transform;
            Transform parent = tr.parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i) == tr)
                {
                    // Subtract one to account for template child.
                    selectedIndex = i - 1;
                    break;
                }
            }

            if (selectedIndex < 0)
                return;

            value = selectedIndex;
            Hide();
        }
    }
}
