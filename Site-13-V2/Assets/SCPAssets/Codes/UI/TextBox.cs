using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Site13Kernel.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Site13Kernel.UI
{
    /// <summary>
    /// Turn a simple label into a interactable input field.
    /// </summary>

    [AddComponentMenu("UI/Customized/Text Field", 31)]
    public partial class TextBox : Selectable, IUpdateSelectedHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler,
        ISubmitHandler, ICanvasElement, ILayoutElement, IEditable, IVisualElement, IPropertiedObject
    {
        public enum ContentType
        {
            Standard,

            Autocorrected,
            IntegerNumber,
            DecimalNumber,
            Alphanumeric,
            Name,
            EmailAddress,
            Password,
            Pin,
            Custom
        }
        public enum InputType
        {
            Standard,
            AutoCorrect,
            Password,
        }

        public enum CharacterValidation
        {
            None,
            Integer,
            Decimal,
            Alphanumeric,
            Name,
            EmailAddress
        }

        public enum LineType
        {
            SingleLine,

            MultiLineSubmit,

            MultiLineNewline
        }

        public delegate char OnValidateInput(string text, int charIndex, char addedChar);

        [Serializable]
        public class SubmitEvent : UnityEvent<string> { }

        [Serializable]
        public class EndEditEvent : UnityEvent<string> { }

        [Serializable]
        public class OnChangeEvent : UnityEvent<string> { }

        protected TouchScreenKeyboard m_Keyboard;
        static private readonly char[] kSeparators = { ' ', '.', ',', '\t', '\r', '\n' };

        [SerializeField]
        [FormerlySerializedAs("text")]
        protected Text m_TextComponent;

        [SerializeField]
        protected Graphic m_Placeholder;

        [SerializeField]
        private ContentType m_ContentType = ContentType.Standard;

        [FormerlySerializedAs("inputType")]
        [SerializeField]
        private InputType m_InputType = InputType.Standard;

        [FormerlySerializedAs("asteriskChar")]
        [SerializeField]
        private char m_AsteriskChar = '*';

        [FormerlySerializedAs("keyboardType")]
        [SerializeField]
        private TouchScreenKeyboardType m_KeyboardType = TouchScreenKeyboardType.Default;

        [SerializeField]
        private LineType m_LineType = LineType.SingleLine;

        [FormerlySerializedAs("hideMobileInput")]
        [SerializeField]
        private bool m_HideMobileInput = false;

        [FormerlySerializedAs("validation")]
        [SerializeField]
        private CharacterValidation m_CharacterValidation = CharacterValidation.None;

        [FormerlySerializedAs("characterLimit")]
        [SerializeField]
        private int m_CharacterLimit = 0;

        [FormerlySerializedAs("onSubmit")]
        [FormerlySerializedAs("m_OnSubmit")]
        [FormerlySerializedAs("m_EndEdit")]
        [FormerlySerializedAs("m_OnEndEdit")]
        [SerializeField]
        private SubmitEvent m_OnSubmit = new SubmitEvent();

        [SerializeField]
        private EndEditEvent m_OnDidEndEdit = new EndEditEvent();

        [FormerlySerializedAs("onValueChange")]
        [FormerlySerializedAs("m_OnValueChange")]
        [SerializeField]
        private OnChangeEvent m_OnValueChanged = new OnChangeEvent();

        [FormerlySerializedAs("onValidateInput")]
        [SerializeField]
        private OnValidateInput m_OnValidateInput;

        [FormerlySerializedAs("selectionColor")]
        [SerializeField]
        private Color m_CaretColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

        [SerializeField]
        private bool m_CustomCaretColor = false;

        [SerializeField]
        private Color m_SelectionColor = new Color(168f / 255f, 206f / 255f, 255f / 255f, 192f / 255f);

        [SerializeField]
        [Multiline]
        [FormerlySerializedAs("mValue")]
        protected string m_Text = string.Empty;

        [SerializeField]
        [Range(0f, 4f)]
        private float m_CaretBlinkRate = 0.85f;

        [SerializeField]
        [Range(1, 5)]
        private int m_CaretWidth = 1;

        [SerializeField]
        private bool m_ReadOnly = false;

        [SerializeField]
        private bool m_ShouldActivateOnSelect = true;

        protected int m_CaretPosition = 0;
        protected int m_CaretSelectPosition = 0;
        private RectTransform caretRectTrans = null;
        protected UIVertex[] m_CursorVerts = null;
        private TextGenerator m_InputTextCache;
        private CanvasRenderer m_CachedInputRenderer;
        private bool m_PreventFontCallback = false;
        [NonSerialized] protected Mesh m_Mesh;
        private bool m_AllowInput = false;
        private bool m_ShouldActivateNextUpdate = false;
        private bool m_UpdateDrag = false;
        private bool m_DragPositionOutOfBounds = false;
        private const float kHScrollSpeed = 0.05f;
        private const float kVScrollSpeed = 0.10f;
        protected bool m_CaretVisible;
        private Coroutine m_BlinkCoroutine = null;
        private float m_BlinkStartTime = 0.0f;
        protected int m_DrawStart = 0;
        protected int m_DrawEnd = 0;
        private Coroutine m_DragCoroutine = null;
        private string m_OriginalText = "";
        private bool m_WasCanceled = false;
        private bool m_HasDoneFocusTransition = false;
        private WaitForSecondsRealtime m_WaitForSecondsRealtime;
        private bool m_TouchKeyboardAllowsInPlaceEditing = false;

        private BaseInput input
        {
            get
            {
                if (EventSystem.current && EventSystem.current.currentInputModule)
                    return EventSystem.current.currentInputModule.input;
                return null;
            }
        }

        private string compositionString
        {
            get { return input != null ? input.compositionString : Input.compositionString; }
        }

        // Doesn't include dot and @ on purpose! See usage for details.
        const string kEmailSpecialCharacters = "!#$%&'*+-/=?^_`{|}~";

        protected TextBox()
        {
            EnforceTextHOverflow();
        }

        protected Mesh mesh
        {
            get
            {
                if (m_Mesh == null)
                    m_Mesh = new Mesh();
                return m_Mesh;
            }
        }

        protected TextGenerator cachedInputTextGenerator
        {
            get
            {
                if (m_InputTextCache == null)
                    m_InputTextCache = new TextGenerator();

                return m_InputTextCache;
            }
        }

        public bool shouldHideMobileInput
        {
            set
            {
                SetPropertyUtility.SetStruct(ref m_HideMobileInput, value);
            }
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                    case RuntimePlatform.IPhonePlayer:
                    case RuntimePlatform.tvOS:
                        return m_HideMobileInput;
                }

                return true;
            }
        }
        public virtual bool shouldActivateOnSelect
        {
            set
            {
                m_ShouldActivateOnSelect = value;
            }
            get
            {
                return m_ShouldActivateOnSelect && Application.platform != RuntimePlatform.tvOS;
            }
        }


        public string text
        {
            get
            {
                return m_Text;
            }
            set
            {
                SetText(value);
            }
        }

        public void SetTextWithoutNotify(string input)
        {
            SetText(input, false);
        }

        void SetText(string value, bool sendCallback = true)
        {
            if (this.text == value)
                return;
            if (value == null)
                value = "";
            value = value.Replace("\0", string.Empty); // remove embedded nulls
            if (m_LineType == LineType.SingleLine)
                value = value.Replace("\n", "").Replace("\t", "");

            // If we have an input validator, validate the input and apply the character limit at the same time.
            if (onValidateInput != null || characterValidation != CharacterValidation.None)
            {
                m_Text = "";
                OnValidateInput validatorMethod = onValidateInput ?? Validate;
                m_CaretPosition = m_CaretSelectPosition = value.Length;
                int charactersToCheck = characterLimit > 0 ? Math.Min(characterLimit, value.Length) : value.Length;
                for (int i = 0; i < charactersToCheck; ++i)
                {
                    char c = validatorMethod(m_Text, m_Text.Length, value[i]);
                    if (c != 0)
                        m_Text += c;
                }
            }
            else
            {
                m_Text = characterLimit > 0 && value.Length > characterLimit ? value.Substring(0, characterLimit) : value;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                SendOnValueChangedAndUpdateLabel();
                return;
            }
#endif

            if (m_Keyboard != null)
                m_Keyboard.text = m_Text;

            if (m_CaretPosition > m_Text.Length)
                m_CaretPosition = m_CaretSelectPosition = m_Text.Length;
            else if (m_CaretSelectPosition > m_Text.Length)
                m_CaretSelectPosition = m_Text.Length;

            if (sendCallback)
                SendOnValueChanged();
            UpdateLabel();
        }
        public bool isFocused
        {
            get { return m_AllowInput; }
        }

        public float caretBlinkRate
        {
            get { return m_CaretBlinkRate; }
            set
            {
                if (SetPropertyUtility.SetStruct(ref m_CaretBlinkRate, value))
                {
                    if (m_AllowInput)
                        SetCaretActive();
                }
            }
        }

        public int caretWidth { get { return m_CaretWidth; } set { if (SetPropertyUtility.SetStruct(ref m_CaretWidth, value)) MarkGeometryAsDirty(); } }

        public Text textComponent
        {
            get { return m_TextComponent; }
            set
            {
                if (m_TextComponent != null)
                {
                    m_TextComponent.UnregisterDirtyVerticesCallback(MarkGeometryAsDirty);
                    m_TextComponent.UnregisterDirtyVerticesCallback(UpdateLabel);
                    m_TextComponent.UnregisterDirtyMaterialCallback(UpdateCaretMaterial);
                }

                if (SetPropertyUtility.SetClass(ref m_TextComponent, value))
                {
                    EnforceTextHOverflow();
                    if (m_TextComponent != null)
                    {
                        m_TextComponent.RegisterDirtyVerticesCallback(MarkGeometryAsDirty);
                        m_TextComponent.RegisterDirtyVerticesCallback(UpdateLabel);
                        m_TextComponent.RegisterDirtyMaterialCallback(UpdateCaretMaterial);
                    }
                }
            }
        }

        public Graphic placeholder { get { return m_Placeholder; } set { SetPropertyUtility.SetClass(ref m_Placeholder, value); } }

        public Color caretColor { get { return customCaretColor ? m_CaretColor : textComponent.color; } set { if (SetPropertyUtility.SetColor(ref m_CaretColor, value)) MarkGeometryAsDirty(); } }

        public bool customCaretColor { get { return m_CustomCaretColor; } set { if (m_CustomCaretColor != value) { m_CustomCaretColor = value; MarkGeometryAsDirty(); } } }

        public Color selectionColor { get { return m_SelectionColor; } set { if (SetPropertyUtility.SetColor(ref m_SelectionColor, value)) MarkGeometryAsDirty(); } }

        public EndEditEvent onEndEdit { get { return m_OnDidEndEdit; } set { SetPropertyUtility.SetClass(ref m_OnDidEndEdit, value); } }

        public SubmitEvent onSubmit { get { return m_OnSubmit; } set { SetPropertyUtility.SetClass(ref m_OnSubmit, value); } }

        [Obsolete("onValueChange has been renamed to onValueChanged")]
        public OnChangeEvent onValueChange { get { return onValueChanged; } set { onValueChanged = value; } }

        public OnChangeEvent onValueChanged { get { return m_OnValueChanged; } set { SetPropertyUtility.SetClass(ref m_OnValueChanged, value); } }

        public OnValidateInput onValidateInput { get { return m_OnValidateInput; } set { SetPropertyUtility.SetClass(ref m_OnValidateInput, value); } }

        public int characterLimit
        {
            get { return m_CharacterLimit; }
            set
            {
                if (SetPropertyUtility.SetStruct(ref m_CharacterLimit, Math.Max(0, value)))
                {
                    UpdateLabel();
                    if (m_Keyboard != null)
                        m_Keyboard.characterLimit = value;
                }
            }
        }

        public ContentType contentType { get { return m_ContentType; } set { if (SetPropertyUtility.SetStruct(ref m_ContentType, value)) EnforceContentType(); } }

        public LineType lineType
        {
            get { return m_LineType; }
            set
            {
                if (SetPropertyUtility.SetStruct(ref m_LineType, value))
                {
                    SetToCustomIfContentTypeIsNot(ContentType.Standard, ContentType.Autocorrected);
                    EnforceTextHOverflow();
                }
            }
        }

        public InputType inputType { get { return m_InputType; } set { if (SetPropertyUtility.SetStruct(ref m_InputType, value)) SetToCustom(); } }

        public TouchScreenKeyboard touchScreenKeyboard { get { return m_Keyboard; } }

        public TouchScreenKeyboardType keyboardType
        {
            get { return m_KeyboardType; }
            set
            {
                if (SetPropertyUtility.SetStruct(ref m_KeyboardType, value))
                    SetToCustom();
            }
        }

        public CharacterValidation characterValidation { get { return m_CharacterValidation; } set { if (SetPropertyUtility.SetStruct(ref m_CharacterValidation, value)) SetToCustom(); } }


        public bool readOnly { get { return m_ReadOnly; } set { m_ReadOnly = value; } }

        public bool multiLine { get { return m_LineType == LineType.MultiLineNewline || lineType == LineType.MultiLineSubmit; } }

        public char asteriskChar { get { return m_AsteriskChar; } set { if (SetPropertyUtility.SetStruct(ref m_AsteriskChar, value)) UpdateLabel(); } }

        public bool wasCanceled { get { return m_WasCanceled; } }

        protected void ClampPos(ref int pos)
        {
            if (pos < 0) pos = 0;
            else if (pos > text.Length) pos = text.Length;
        }


        protected int caretPositionInternal { get { return m_CaretPosition + compositionString.Length; } set { m_CaretPosition = value; ClampPos(ref m_CaretPosition); } }
        protected int caretSelectPositionInternal { get { return m_CaretSelectPosition + compositionString.Length; } set { m_CaretSelectPosition = value; ClampPos(ref m_CaretSelectPosition); } }
        private bool hasSelection { get { return caretPositionInternal != caretSelectPositionInternal; } }

#if UNITY_EDITOR
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("caretSelectPosition has been deprecated. Use selectionFocusPosition instead (UnityUpgradable) -> selectionFocusPosition", true)]
        public int caretSelectPosition { get { return selectionFocusPosition; } protected set { selectionFocusPosition = value; } }
#endif


        public int caretPosition
        {
            get { return m_CaretSelectPosition + compositionString.Length; }
            set { selectionAnchorPosition = value; selectionFocusPosition = value; }
        }
        public int selectionAnchorPosition
        {
            get { return m_CaretPosition + compositionString.Length; }
            set
            {
                if (compositionString.Length != 0)
                    return;

                m_CaretPosition = value;
                ClampPos(ref m_CaretPosition);
            }
        }
        public int selectionFocusPosition
        {
            get { return m_CaretSelectPosition + compositionString.Length; }
            set
            {
                if (compositionString.Length != 0)
                    return;

                m_CaretSelectPosition = value;
                ClampPos(ref m_CaretSelectPosition);
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            EnforceContentType();
            EnforceTextHOverflow();

            m_CharacterLimit = Math.Max(0, m_CharacterLimit);

            //This can be invoked before OnEnabled is called. So we shouldn't be accessing other objects, before OnEnable is called.
            if (!IsActive())
                return;

            // fix case 1040277
            ClampPos(ref m_CaretPosition);
            ClampPos(ref m_CaretSelectPosition);


            UpdateLabel();
            if (m_AllowInput)
                SetCaretActive();
        }

#endif // if UNITY_EDITOR

        protected override void OnEnable()
        {
            base.OnEnable();
            if (m_Text == null)
                m_Text = string.Empty;
            m_DrawStart = 0;
            m_DrawEnd = m_Text.Length;

            // If we have a cached renderer then we had OnDisable called so just restore the material.
            if (m_CachedInputRenderer != null)
                m_CachedInputRenderer.SetMaterial(m_TextComponent.GetModifiedMaterial(Graphic.defaultGraphicMaterial), Texture2D.whiteTexture);

            if (m_TextComponent != null)
            {
                m_TextComponent.RegisterDirtyVerticesCallback(MarkGeometryAsDirty);
                m_TextComponent.RegisterDirtyVerticesCallback(UpdateLabel);
                m_TextComponent.RegisterDirtyMaterialCallback(UpdateCaretMaterial);
                UpdateLabel();
            }
        }

        protected override void OnDisable()
        {
            // the coroutine will be terminated, so this will ensure it restarts when we are next activated
            m_BlinkCoroutine = null;

            DeactivateInputField();
            if (m_TextComponent != null)
            {
                m_TextComponent.UnregisterDirtyVerticesCallback(MarkGeometryAsDirty);
                m_TextComponent.UnregisterDirtyVerticesCallback(UpdateLabel);
                m_TextComponent.UnregisterDirtyMaterialCallback(UpdateCaretMaterial);
            }
            CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);

            // Clear needs to be called otherwise sync never happens as the object is disabled.
            if (m_CachedInputRenderer != null)
                m_CachedInputRenderer.Clear();

            if (m_Mesh != null)
                DestroyImmediate(m_Mesh);
            m_Mesh = null;

            base.OnDisable();
        }

        IEnumerator CaretBlink()
        {
            // Always ensure caret is initially visible since it can otherwise be confusing for a moment.
            m_CaretVisible = true;
            yield return null;

            while (isFocused && m_CaretBlinkRate > 0)
            {
                // the blink rate is expressed as a frequency
                float blinkPeriod = 1f / m_CaretBlinkRate;

                // the caret should be ON if we are in the first half of the blink period
                bool blinkState = (Time.unscaledTime - m_BlinkStartTime) % blinkPeriod < blinkPeriod / 2;
                if (m_CaretVisible != blinkState)
                {
                    m_CaretVisible = blinkState;
                    if (!hasSelection)
                        MarkGeometryAsDirty();
                }

                // Then wait again.
                yield return null;
            }
            m_BlinkCoroutine = null;
        }

        void SetCaretVisible()
        {
            if (!m_AllowInput)
                return;

            m_CaretVisible = true;
            m_BlinkStartTime = Time.unscaledTime;
            SetCaretActive();
        }

        // SetCaretActive will not set the caret immediately visible - it will wait for the next time to blink.
        // However, it will handle things correctly if the blink speed changed from zero to non-zero or non-zero to zero.
        void SetCaretActive()
        {
            if (!m_AllowInput)
                return;

            if (m_CaretBlinkRate > 0.0f)
            {
                if (m_BlinkCoroutine == null)
                    m_BlinkCoroutine = StartCoroutine(CaretBlink());
            }
            else
            {
                m_CaretVisible = true;
            }
        }

        private void UpdateCaretMaterial()
        {
            if (m_TextComponent != null && m_CachedInputRenderer != null)
                m_CachedInputRenderer.SetMaterial(m_TextComponent.GetModifiedMaterial(Graphic.defaultGraphicMaterial), Texture2D.whiteTexture);
        }

        protected void OnFocus()
        {
            SelectAll();
        }
        protected void SelectAll()
        {
            caretPositionInternal = text.Length;
            caretSelectPositionInternal = 0;
        }
        public void MoveTextEnd(bool shift)
        {
            int position = text.Length;

            if (shift)
            {
                caretSelectPositionInternal = position;
            }
            else
            {
                caretPositionInternal = position;
                caretSelectPositionInternal = caretPositionInternal;
            }
            UpdateLabel();
        }

        public void MoveTextStart(bool shift)
        {
            int position = 0;

            if (shift)
            {
                caretSelectPositionInternal = position;
            }
            else
            {
                caretPositionInternal = position;
                caretSelectPositionInternal = caretPositionInternal;
            }

            UpdateLabel();
        }

        static string clipboard
        {
            get
            {
                return GUIUtility.systemCopyBuffer;
            }
            set
            {
                GUIUtility.systemCopyBuffer = value;
            }
        }

        private bool TouchScreenKeyboardShouldBeUsed()
        {
            RuntimePlatform platform = Application.platform;
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return !TouchScreenKeyboard.isInPlaceEditingAllowed;
                default:
                    return TouchScreenKeyboard.isSupported;
            }
        }

        private bool InPlaceEditing()
        {
            return !TouchScreenKeyboard.isSupported || m_TouchKeyboardAllowsInPlaceEditing;
        }

        private bool InPlaceEditingChanged()
        {
            return m_TouchKeyboardAllowsInPlaceEditing != TouchScreenKeyboard.isInPlaceEditingAllowed;
        }

        void UpdateCaretFromKeyboard()
        {
            var selectionRange = m_Keyboard.selection;

            var selectionStart = selectionRange.start;
            var selectionEnd = selectionRange.end;

            var caretChanged = false;

            if (caretPositionInternal != selectionStart)
            {
                caretChanged = true;
                caretPositionInternal = selectionStart;
            }

            if (caretSelectPositionInternal != selectionEnd)
            {
                caretSelectPositionInternal = selectionEnd;
                caretChanged = true;
            }

            if (caretChanged)
            {
                m_BlinkStartTime = Time.unscaledTime;

                UpdateLabel();
            }
        }

        protected virtual void LateUpdate()
        {
            // Only activate if we are not already activated.
            if (m_ShouldActivateNextUpdate)
            {
                if (!isFocused)
                {
                    ActivateInputFieldInternal();
                    m_ShouldActivateNextUpdate = false;
                    return;
                }

                // Reset as we are already activated.
                m_ShouldActivateNextUpdate = false;
            }

            AssignPositioningIfNeeded();

            // If the device's state changed in a way that affects whether we should use a touchscreen keyboard or not,
            // then we make sure to clear all of the caret/highlight state visually and deactivate the input field.
            if (isFocused && InPlaceEditingChanged())
            {
                if (m_CachedInputRenderer != null)
                {
                    using (var helper = new VertexHelper())
                        helper.FillMesh(mesh);

                    m_CachedInputRenderer.SetMesh(mesh);
                }

                DeactivateInputField();
            }

            if (!isFocused || InPlaceEditing())
                return;

            if (m_Keyboard == null || m_Keyboard.status != TouchScreenKeyboard.Status.Visible)
            {
                if (m_Keyboard != null)
                {
                    if (!m_ReadOnly)
                        text = m_Keyboard.text;

                    if (m_Keyboard.status == TouchScreenKeyboard.Status.Canceled)
                        m_WasCanceled = true;
                    else if (m_Keyboard.status == TouchScreenKeyboard.Status.Done)
                        SendOnSubmit();
                }

                OnDeselect(null);
                return;
            }

            string val = m_Keyboard.text;

            if (m_Text != val)
            {
                if (m_ReadOnly)
                {
                    m_Keyboard.text = m_Text;
                }
                else
                {
                    m_Text = "";

                    for (int i = 0; i < val.Length; ++i)
                    {
                        char c = val[i];

                        if (c == '\r' || (int)c == 3)
                            c = '\n';

                        if (onValidateInput != null)
                            c = onValidateInput(m_Text, m_Text.Length, c);
                        else if (characterValidation != CharacterValidation.None)
                            c = Validate(m_Text, m_Text.Length, c);

                        if (lineType == LineType.MultiLineSubmit && c == '\n')
                        {
                            m_Keyboard.text = m_Text;

                            SendOnSubmit();
                            OnDeselect(null);
                            return;
                        }

                        if (c != 0)
                            m_Text += c;
                    }

                    if (characterLimit > 0 && m_Text.Length > characterLimit)
                        m_Text = m_Text.Substring(0, characterLimit);

                    if (m_Keyboard.canGetSelection)
                    {
                        UpdateCaretFromKeyboard();
                    }
                    else
                    {
                        caretPositionInternal = caretSelectPositionInternal = m_Text.Length;
                    }

                    // Set keyboard text before updating label, as we might have changed it with validation
                    // and update label will take the old value from keyboard if we don't change it here
                    if (m_Text != val)
                        m_Keyboard.text = m_Text;

                    SendOnValueChangedAndUpdateLabel();
                }
            }
            else if (m_HideMobileInput && m_Keyboard.canSetSelection)
            {
                var selectionStart = Mathf.Min(caretSelectPositionInternal, caretPositionInternal);
                var selectionLength = Mathf.Abs(caretSelectPositionInternal - caretPositionInternal);
                m_Keyboard.selection = new RangeInt(selectionStart, selectionLength);
            }
            else if (m_Keyboard.canGetSelection && !m_HideMobileInput)
            {
                UpdateCaretFromKeyboard();
            }


            if (m_Keyboard.status != TouchScreenKeyboard.Status.Visible)
            {
                if (m_Keyboard.status == TouchScreenKeyboard.Status.Canceled)
                    m_WasCanceled = true;
                else if (m_Keyboard.status == TouchScreenKeyboard.Status.Done)
                    SendOnSubmit();

                OnDeselect(null);
            }
        }

        [Obsolete("This function is no longer used. Please use RectTransformUtility.ScreenPointToLocalPointInRectangle() instead.")]
        public Vector2 ScreenToLocal(Vector2 screen)
        {
            var theCanvas = m_TextComponent.canvas;
            if (theCanvas == null)
                return screen;

            Vector3 pos = Vector3.zero;
            if (theCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                pos = m_TextComponent.transform.InverseTransformPoint(screen);
            }
            else if (theCanvas.worldCamera != null)
            {
                Ray mouseRay = theCanvas.worldCamera.ScreenPointToRay(screen);
                float dist;
                Plane plane = new Plane(m_TextComponent.transform.forward, m_TextComponent.transform.position);
                plane.Raycast(mouseRay, out dist);
                pos = m_TextComponent.transform.InverseTransformPoint(mouseRay.GetPoint(dist));
            }
            return new Vector2(pos.x, pos.y);
        }

        private int GetUnclampedCharacterLineFromPosition(Vector2 pos, TextGenerator generator)
        {
            if (!multiLine)
                return 0;

            // transform y to local scale
            float y = pos.y * m_TextComponent.pixelsPerUnit;
            float lastBottomY = 0.0f;

            for (int i = 0; i < generator.lineCount; ++i)
            {
                float topY = generator.lines[i].topY;
                float bottomY = topY - generator.lines[i].height;

                // pos is somewhere in the leading above this line
                if (y > topY)
                {
                    // determine which line we're closer to
                    float leading = topY - lastBottomY;
                    if (y > topY - 0.5f * leading)
                        return i - 1;
                    else
                        return i;
                }

                if (y > bottomY)
                    return i;

                lastBottomY = bottomY;
            }

            // Position is after last line.
            return generator.lineCount;
        }

        protected int GetCharacterIndexFromPosition(Vector2 pos)
        {
            TextGenerator gen = m_TextComponent.cachedTextGenerator;

            if (gen.lineCount == 0)
                return 0;

            int line = GetUnclampedCharacterLineFromPosition(pos, gen);
            if (line < 0)
                return 0;
            if (line >= gen.lineCount)
                return gen.characterCountVisible;

            int startCharIndex = gen.lines[line].startCharIdx;
            int endCharIndex = GetLineEndPosition(gen, line);

            for (int i = startCharIndex; i < endCharIndex; i++)
            {
                if (i >= gen.characterCountVisible)
                    break;

                UICharInfo charInfo = gen.characters[i];
                Vector2 charPos = charInfo.cursorPos / m_TextComponent.pixelsPerUnit;

                float distToCharStart = pos.x - charPos.x;
                float distToCharEnd = charPos.x + (charInfo.charWidth / m_TextComponent.pixelsPerUnit) - pos.x;
                if (distToCharStart < distToCharEnd)
                    return i;
            }

            return endCharIndex;
        }

        private bool MayDrag(PointerEventData eventData)
        {
            return IsActive() &&
                IsInteractable() &&
                eventData.button == PointerEventData.InputButton.Left &&
                m_TextComponent != null &&
                (InPlaceEditing() || m_HideMobileInput);
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (!MayDrag(eventData))
                return;

            m_UpdateDrag = true;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!MayDrag(eventData))
                return;

            Vector2 position = Vector2.zero;
            if (!MultipleDisplayUtilities.GetRelativeMousePositionForDrag(eventData, ref position))
                return;

            Vector2 localMousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(textComponent.rectTransform, position, eventData.pressEventCamera, out localMousePos);
            caretSelectPositionInternal = GetCharacterIndexFromPosition(localMousePos) + m_DrawStart;

            MarkGeometryAsDirty();

            m_DragPositionOutOfBounds = !RectTransformUtility.RectangleContainsScreenPoint(textComponent.rectTransform, eventData.position, eventData.pressEventCamera);
            if (m_DragPositionOutOfBounds && m_DragCoroutine == null)
                m_DragCoroutine = StartCoroutine(MouseDragOutsideRect(eventData));

            eventData.Use();
        }

        IEnumerator MouseDragOutsideRect(PointerEventData eventData)
        {
            while (m_UpdateDrag && m_DragPositionOutOfBounds)
            {
                Vector2 position = Vector2.zero;
                if (!MultipleDisplayUtilities.GetRelativeMousePositionForDrag(eventData, ref position))
                    break;

                Vector2 localMousePos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(textComponent.rectTransform, position, eventData.pressEventCamera, out localMousePos);

                Rect rect = textComponent.rectTransform.rect;

                if (multiLine)
                {
                    if (localMousePos.y > rect.yMax)
                        MoveUp(true, true);
                    else if (localMousePos.y < rect.yMin)
                        MoveDown(true, true);
                }
                else
                {
                    if (localMousePos.x < rect.xMin)
                        MoveLeft(true, false);
                    else if (localMousePos.x > rect.xMax)
                        MoveRight(true, false);
                }
                UpdateLabel();
                float delay = multiLine ? kVScrollSpeed : kHScrollSpeed;
                if (m_WaitForSecondsRealtime == null)
                    m_WaitForSecondsRealtime = new WaitForSecondsRealtime(delay);
                else
                    m_WaitForSecondsRealtime.waitTime = delay;
                yield return m_WaitForSecondsRealtime;
            }
            m_DragCoroutine = null;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (!MayDrag(eventData))
                return;

            m_UpdateDrag = false;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!MayDrag(eventData))
                return;

            EventSystem.current.SetSelectedGameObject(gameObject, eventData);

            bool hadFocusBefore = m_AllowInput;
            base.OnPointerDown(eventData);

            if (!InPlaceEditing())
            {
                if (m_Keyboard == null || !m_Keyboard.active)
                {
                    OnSelect(eventData);
                    return;
                }
            }

            // Only set caret position if we didn't just get focus now.
            // Otherwise it will overwrite the select all on focus.
            if (hadFocusBefore)
            {
                Vector2 localMousePos;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(textComponent.rectTransform, eventData.pointerPressRaycast.screenPosition, eventData.pressEventCamera, out localMousePos);
                caretSelectPositionInternal = caretPositionInternal = GetCharacterIndexFromPosition(localMousePos) + m_DrawStart;
            }

            UpdateLabel();
            eventData.Use();
        }

        protected enum EditState
        {
            Continue,
            Finish
        }


        /// <summary>
        /// Process the Event and perform the appropriate action for that key.
        /// </summary>
        /// <param name="evt">The Event that is currently being processed.</param>
        /// <returns>If we should continue processing events or we have hit an end condition.</returns>
        protected EditState KeyPressed(Event evt)
        {
            var currentEventModifiers = evt.modifiers;
            bool ctrl = SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX ? (currentEventModifiers & EventModifiers.Command) != 0 : (currentEventModifiers & EventModifiers.Control) != 0;
            bool shift = (currentEventModifiers & EventModifiers.Shift) != 0;
            bool alt = (currentEventModifiers & EventModifiers.Alt) != 0;
            bool ctrlOnly = ctrl && !alt && !shift;
            bool shiftOnly = shift && !ctrl && !alt;

            switch (evt.keyCode)
            {
                case KeyCode.Backspace:
                    {
                        Backspace();
                        return EditState.Continue;
                    }

                case KeyCode.Delete:
                    {
                        ForwardSpace();
                        return EditState.Continue;
                    }

                case KeyCode.Home:
                    {
                        MoveTextStart(shift);
                        return EditState.Continue;
                    }

                case KeyCode.End:
                    {
                        MoveTextEnd(shift);
                        return EditState.Continue;
                    }

                // Select All
                case KeyCode.A:
                    {
                        if (ctrlOnly)
                        {
                            SelectAll();
                            return EditState.Continue;
                        }
                        break;
                    }

                // Copy
                case KeyCode.C:
                    {
                        if (ctrlOnly)
                        {
                            if (inputType != InputType.Password)
                                clipboard = GetSelectedString();
                            else
                                clipboard = "";
                            return EditState.Continue;
                        }
                        break;
                    }

                // Paste
                case KeyCode.V:
                    {
                        if (ctrlOnly)
                        {
                            Append(clipboard);
                            UpdateLabel();
                            return EditState.Continue;
                        }
                        break;
                    }

                // Cut
                case KeyCode.X:
                    {
                        if (ctrlOnly)
                        {
                            if (inputType != InputType.Password)
                                clipboard = GetSelectedString();
                            else
                                clipboard = "";

                            Delete();
                            UpdateTouchKeyboardFromEditChanges();
                            SendOnValueChangedAndUpdateLabel();
                            return EditState.Continue;
                        }
                        break;
                    }
                case KeyCode.Insert:
                    {
                        // Copy via Insert key
                        if (ctrlOnly)
                        {
                            if (inputType != InputType.Password)
                                clipboard = GetSelectedString();
                            else
                                clipboard = "";
                            return EditState.Continue;
                        }
                        // Paste via insert key.
                        else if (shiftOnly)
                        {
                            Append(clipboard);
                            UpdateLabel();
                            return EditState.Continue;
                        }
                        break;
                    }

                case KeyCode.LeftArrow:
                    {
                        MoveLeft(shift, ctrl);
                        return EditState.Continue;
                    }

                case KeyCode.RightArrow:
                    {
                        MoveRight(shift, ctrl);
                        return EditState.Continue;
                    }

                case KeyCode.UpArrow:
                    {
                        MoveUp(shift);
                        return EditState.Continue;
                    }

                case KeyCode.DownArrow:
                    {
                        MoveDown(shift);
                        return EditState.Continue;
                    }

                // Submit
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    {
                        if (lineType != LineType.MultiLineNewline)
                        {
                            return EditState.Finish;
                        }
                        break;
                    }

                case KeyCode.Escape:
                    {
                        m_WasCanceled = true;
                        return EditState.Finish;
                    }
            }

            char c = evt.character;
            // Don't allow return chars or tabulator key to be entered into single line fields.
            if (!multiLine && (c == '\t' || c == '\r' || c == 10))
                return EditState.Continue;

            // Convert carriage return and end-of-text characters to newline.
            if (c == '\r' || (int)c == 3)
                c = '\n';

            if (IsValidChar(c))
            {
                Append(c);
            }

            if (c == 0)
            {
                if (compositionString.Length > 0)
                {
                    UpdateLabel();
                }
            }
            return EditState.Continue;
        }

        private bool IsValidChar(char c)
        {
            // Delete key on mac
            if ((int)c == 127)
                return false;
            // Accept newline and tab
            if (c == '\t' || c == '\n')
                return true;

            return m_TextComponent.font.HasCharacter(c);
        }

        /// <summary>
        /// Handle the specified event.
        /// </summary>
        private Event m_ProcessingEvent = new Event();

        /// <summary>
        /// Helper function to allow separate events to be processed by the InputField.
        /// </summary>
        /// <param name="e">The Event to process</param>
        public void ProcessEvent(Event e)
        {
            KeyPressed(e);
        }

        /// <summary>
        /// What to do when the event system sends a Update selected Event.
        /// </summary>
        /// <param name="eventData">The data on which to process.</param>
        public virtual void OnUpdateSelected(BaseEventData eventData)
        {
            if (!isFocused)
                return;

            bool consumedEvent = false;
            while (Event.PopEvent(m_ProcessingEvent))
            {
                if (m_ProcessingEvent.rawType == EventType.KeyDown)
                {
                    consumedEvent = true;
                    var shouldContinue = KeyPressed(m_ProcessingEvent);
                    if (shouldContinue == EditState.Finish)
                    {
                        if (!m_WasCanceled)
                            SendOnSubmit();

                        DeactivateInputField();
                        break;
                    }
                }

                switch (m_ProcessingEvent.type)
                {
                    case EventType.ValidateCommand:
                    case EventType.ExecuteCommand:
                        switch (m_ProcessingEvent.commandName)
                        {
                            case "SelectAll":
                                SelectAll();
                                consumedEvent = true;
                                break;
                        }
                        break;
                }
            }

            if (consumedEvent)
                UpdateLabel();

            eventData.Use();
        }

        private string GetSelectedString()
        {
            if (!hasSelection)
                return "";

            int startPos = caretPositionInternal;
            int endPos = caretSelectPositionInternal;

            // Ensure startPos is always less then endPos to make the code simpler
            if (startPos > endPos)
            {
                int temp = startPos;
                startPos = endPos;
                endPos = temp;
            }

            return text.Substring(startPos, endPos - startPos);
        }

        private int FindtNextWordBegin()
        {
            if (caretSelectPositionInternal + 1 >= text.Length)
                return text.Length;

            int spaceLoc = text.IndexOfAny(kSeparators, caretSelectPositionInternal + 1);

            if (spaceLoc == -1)
                spaceLoc = text.Length;
            else
                spaceLoc++;

            return spaceLoc;
        }

        private void MoveRight(bool shift, bool ctrl)
        {
            if (hasSelection && !shift)
            {
                // By convention, if we have a selection and move right without holding shift,
                // we just place the cursor at the end.
                caretPositionInternal = caretSelectPositionInternal = Mathf.Max(caretPositionInternal, caretSelectPositionInternal);
                return;
            }

            int position;
            if (ctrl)
                position = FindtNextWordBegin();
            else
                position = caretSelectPositionInternal + 1;

            if (shift)
                caretSelectPositionInternal = position;
            else
                caretSelectPositionInternal = caretPositionInternal = position;
        }

        private int FindtPrevWordBegin()
        {
            if (caretSelectPositionInternal - 2 < 0)
                return 0;

            int spaceLoc = text.LastIndexOfAny(kSeparators, caretSelectPositionInternal - 2);

            if (spaceLoc == -1)
                spaceLoc = 0;
            else
                spaceLoc++;

            return spaceLoc;
        }

        private void MoveLeft(bool shift, bool ctrl)
        {
            if (hasSelection && !shift)
            {
                // By convention, if we have a selection and move left without holding shift,
                // we just place the cursor at the start.
                caretPositionInternal = caretSelectPositionInternal = Mathf.Min(caretPositionInternal, caretSelectPositionInternal);
                return;
            }

            int position;
            if (ctrl)
                position = FindtPrevWordBegin();
            else
                position = caretSelectPositionInternal - 1;

            if (shift)
                caretSelectPositionInternal = position;
            else
                caretSelectPositionInternal = caretPositionInternal = position;
        }

        private int DetermineCharacterLine(int charPos, TextGenerator generator)
        {
            for (int i = 0; i < generator.lineCount - 1; ++i)
            {
                if (generator.lines[i + 1].startCharIdx > charPos)
                    return i;
            }

            return generator.lineCount - 1;
        }

        /// <summary>
        ///  Use cachedInputTextGenerator as the y component for the UICharInfo is not required
        /// </summary>

        private int LineUpCharacterPosition(int originalPos, bool goToFirstChar)
        {
            if (originalPos >= cachedInputTextGenerator.characters.Count)
                return 0;

            UICharInfo originChar = cachedInputTextGenerator.characters[originalPos];
            int originLine = DetermineCharacterLine(originalPos, cachedInputTextGenerator);

            // We are on the first line return first character
            if (originLine <= 0)
                return goToFirstChar ? 0 : originalPos;

            int endCharIdx = cachedInputTextGenerator.lines[originLine].startCharIdx - 1;

            for (int i = cachedInputTextGenerator.lines[originLine - 1].startCharIdx; i < endCharIdx; ++i)
            {
                if (cachedInputTextGenerator.characters[i].cursorPos.x >= originChar.cursorPos.x)
                    return i;
            }
            return endCharIdx;
        }

        /// <summary>
        ///  Use cachedInputTextGenerator as the y component for the UICharInfo is not required
        /// </summary>

        private int LineDownCharacterPosition(int originalPos, bool goToLastChar)
        {
            if (originalPos >= cachedInputTextGenerator.characterCountVisible)
                return text.Length;

            UICharInfo originChar = cachedInputTextGenerator.characters[originalPos];
            int originLine = DetermineCharacterLine(originalPos, cachedInputTextGenerator);

            // We are on the last line return last character
            if (originLine + 1 >= cachedInputTextGenerator.lineCount)
                return goToLastChar ? text.Length : originalPos;

            // Need to determine end line for next line.
            int endCharIdx = GetLineEndPosition(cachedInputTextGenerator, originLine + 1);

            for (int i = cachedInputTextGenerator.lines[originLine + 1].startCharIdx; i < endCharIdx; ++i)
            {
                if (cachedInputTextGenerator.characters[i].cursorPos.x >= originChar.cursorPos.x)
                    return i;
            }
            return endCharIdx;
        }

        private void MoveDown(bool shift)
        {
            MoveDown(shift, true);
        }

        private void MoveDown(bool shift, bool goToLastChar)
        {
            if (hasSelection && !shift)
            {
                // If we have a selection and press down without shift,
                // set caret position to end of selection before we move it down.
                caretPositionInternal = caretSelectPositionInternal = Mathf.Max(caretPositionInternal, caretSelectPositionInternal);
            }

            int position = multiLine ? LineDownCharacterPosition(caretSelectPositionInternal, goToLastChar) : text.Length;

            if (shift)
                caretSelectPositionInternal = position;
            else
                caretPositionInternal = caretSelectPositionInternal = position;
        }

        private void MoveUp(bool shift)
        {
            MoveUp(shift, true);
        }

        private void MoveUp(bool shift, bool goToFirstChar)
        {
            if (hasSelection && !shift)
            {
                // If we have a selection and press up without shift,
                // set caret position to start of selection before we move it up.
                caretPositionInternal = caretSelectPositionInternal = Mathf.Min(caretPositionInternal, caretSelectPositionInternal);
            }

            int position = multiLine ? LineUpCharacterPosition(caretSelectPositionInternal, goToFirstChar) : 0;

            if (shift)
                caretSelectPositionInternal = position;
            else
                caretSelectPositionInternal = caretPositionInternal = position;
        }

        private void Delete()
        {
            if (m_ReadOnly)
                return;

            if (caretPositionInternal == caretSelectPositionInternal)
                return;

            if (caretPositionInternal < caretSelectPositionInternal)
            {
                m_Text = text.Substring(0, caretPositionInternal) + text.Substring(caretSelectPositionInternal, text.Length - caretSelectPositionInternal);
                caretSelectPositionInternal = caretPositionInternal;
            }
            else
            {
                m_Text = text.Substring(0, caretSelectPositionInternal) + text.Substring(caretPositionInternal, text.Length - caretPositionInternal);
                caretPositionInternal = caretSelectPositionInternal;
            }
        }

        private void ForwardSpace()
        {
            if (m_ReadOnly)
                return;

            if (hasSelection)
            {
                Delete();
                UpdateTouchKeyboardFromEditChanges();
                SendOnValueChangedAndUpdateLabel();
            }
            else
            {
                if (caretPositionInternal < text.Length)
                {
                    m_Text = text.Remove(caretPositionInternal, 1);

                    UpdateTouchKeyboardFromEditChanges();
                    SendOnValueChangedAndUpdateLabel();
                }
            }
        }

        private void Backspace()
        {
            if (m_ReadOnly)
                return;

            if (hasSelection)
            {
                Delete();
                UpdateTouchKeyboardFromEditChanges();
                SendOnValueChangedAndUpdateLabel();
            }
            else
            {
                if (caretPositionInternal > 0 && caretPositionInternal - 1 < text.Length)
                {
                    m_Text = text.Remove(caretPositionInternal - 1, 1);
                    caretSelectPositionInternal = caretPositionInternal = caretPositionInternal - 1;

                    UpdateTouchKeyboardFromEditChanges();
                    SendOnValueChangedAndUpdateLabel();
                }
            }
        }

        // Insert the character and update the label.
        private void Insert(char c)
        {
            if (m_ReadOnly)
                return;

            string replaceString = c.ToString();
            Delete();

            // Can't go past the character limit
            if (characterLimit > 0 && text.Length >= characterLimit)
                return;

            m_Text = text.Insert(m_CaretPosition, replaceString);
            caretSelectPositionInternal = caretPositionInternal += replaceString.Length;

            UpdateTouchKeyboardFromEditChanges();
            SendOnValueChanged();
        }

        private void UpdateTouchKeyboardFromEditChanges()
        {
            // Update the TouchKeyboard's text from edit changes
            // if in-place editing is allowed
            if (m_Keyboard != null && InPlaceEditing())
            {
                m_Keyboard.text = m_Text;
            }
        }

        private void SendOnValueChangedAndUpdateLabel()
        {
            SendOnValueChanged();
            UpdateLabel();
        }

        private void SendOnValueChanged()
        {
            UISystemProfilerApi.AddMarker("InputField.value", this);
            if (onValueChanged != null)
                onValueChanged.Invoke(text);
        }

        /// <summary>
        /// Convenience function to make functionality to send the ::ref::EndEditEvent easier.
        /// </summary>
        protected void SendOnEndEdit()
        {
            UISystemProfilerApi.AddMarker("InputField.onEndEdit", this);
            if (onEndEdit != null)
                onEndEdit.Invoke(m_Text);
        }

        /// <summary>
        /// Convenience function to make functionality to send the ::ref::SubmitEvent easier.
        /// </summary>
        protected void SendOnSubmit()
        {
            UISystemProfilerApi.AddMarker("InputField.onSubmit", this);
            if (onSubmit != null)
            {
                onSubmit.Invoke(m_Text);
                Callback();
            }
        }

        /// <summary>
        /// Append the specified text to the end of the current text string. Appends character by character testing validation criteria.
        /// </summary>
        /// <param name="input">The String to append.</param>
        protected virtual void Append(string input)
        {
            if (m_ReadOnly)
                return;

            if (!InPlaceEditing())
                return;

            for (int i = 0, imax = input.Length; i < imax; ++i)
            {
                char c = input[i];

                if (c >= ' ' || c == '\t' || c == '\r' || c == 10 || c == '\n')
                {
                    Append(c);
                }
            }
        }

        // cf. TextGenerator.cpp
        private const int k_MaxTextLength = UInt16.MaxValue / 4 - 1;

        /// <summary>
        /// Append a character to the input field, taking into account the validation of each character.
        /// </summary>
        /// <param name="input">Character to append.</param>
        protected virtual void Append(char input)
        {
            // We do not currently support surrogate pairs
            if (char.IsSurrogate(input))
                return;

            if (m_ReadOnly || text.Length >= k_MaxTextLength)
                return;

            if (!InPlaceEditing())
                return;

            // If we have an input validator, validate the input first
            int insertionPoint = Math.Min(selectionFocusPosition, selectionAnchorPosition);

            //Get the text based on selection for validation instead of whole text(case 1253193).
            var validateText = text;
            if (selectionFocusPosition != selectionAnchorPosition)
            {
                if (caretPositionInternal < caretSelectPositionInternal)
                {
                    validateText = text.Substring(0, caretPositionInternal) + text.Substring(caretSelectPositionInternal, text.Length - caretSelectPositionInternal);
                }
                else
                {
                    validateText = text.Substring(0, caretSelectPositionInternal) + text.Substring(caretPositionInternal, text.Length - caretPositionInternal);
                }
            }

            if (onValidateInput != null)
                input = onValidateInput(validateText, insertionPoint, input);
            else if (characterValidation != CharacterValidation.None)
                input = Validate(validateText, insertionPoint, input);

            // If the input is invalid, skip it
            if (input == 0)
                return;

            // Append the character and update the label
            Insert(input);
        }

        /// <summary>
        /// Update the Text associated with this input field.
        /// </summary>
        protected void UpdateLabel()
        {
            if (m_TextComponent != null && m_TextComponent.font != null && !m_PreventFontCallback)
            {
                // TextGenerator.Populate invokes a callback that's called for anything
                // that needs to be updated when the data for that font has changed.
                // This makes all Text components that use that font update their vertices.
                // In turn, this makes the InputField that's associated with that Text component
                // update its label by calling this UpdateLabel method.
                // This is a recursive call we want to prevent, since it makes the InputField
                // update based on font data that didn't yet finish executing, or alternatively
                // hang on infinite recursion, depending on whether the cached value is cached
                // before or after the calculation.
                //
                // This callback also occurs when assigning text to our Text component, i.e.,
                // m_TextComponent.text = processed;

                m_PreventFontCallback = true;

                string fullText;
                if (EventSystem.current != null && gameObject == EventSystem.current.currentSelectedGameObject && compositionString.Length > 0)
                    fullText = text.Substring(0, m_CaretPosition) + compositionString + text.Substring(m_CaretPosition);
                else
                    fullText = text;

                string processed;
                if (inputType == InputType.Password)
                    processed = new string(asteriskChar, fullText.Length);
                else
                    processed = fullText;

                bool isEmpty = string.IsNullOrEmpty(fullText);

                if (m_Placeholder != null)
                    m_Placeholder.enabled = isEmpty;

                // If not currently editing the text, set the visible range to the whole text.
                // The UpdateLabel method will then truncate it to the part that fits inside the Text area.
                // We can't do this when text is being edited since it would discard the current scroll,
                // which is defined by means of the m_DrawStart and m_DrawEnd indices.
                if (!m_AllowInput)
                {
                    m_DrawStart = 0;
                    m_DrawEnd = m_Text.Length;
                }

                if (!isEmpty)
                {
                    // Determine what will actually fit into the given line
                    Vector2 extents = m_TextComponent.rectTransform.rect.size;

                    var settings = m_TextComponent.GetGenerationSettings(extents);
                    settings.generateOutOfBounds = true;

                    cachedInputTextGenerator.PopulateWithErrors(processed, settings, gameObject);

                    SetDrawRangeToContainCaretPosition(caretSelectPositionInternal);

                    processed = processed.Substring(m_DrawStart, Mathf.Min(m_DrawEnd, processed.Length) - m_DrawStart);

                    SetCaretVisible();
                }
                m_TextComponent.text = processed;
                MarkGeometryAsDirty();
                m_PreventFontCallback = false;
            }
        }

        private bool IsSelectionVisible()
        {
            if (m_DrawStart > caretPositionInternal || m_DrawStart > caretSelectPositionInternal)
                return false;

            if (m_DrawEnd < caretPositionInternal || m_DrawEnd < caretSelectPositionInternal)
                return false;

            return true;
        }

        private static int GetLineStartPosition(TextGenerator gen, int line)
        {
            line = Mathf.Clamp(line, 0, gen.lines.Count - 1);
            return gen.lines[line].startCharIdx;
        }

        private static int GetLineEndPosition(TextGenerator gen, int line)
        {
            line = Mathf.Max(line, 0);
            if (line + 1 < gen.lines.Count)
                return gen.lines[line + 1].startCharIdx - 1;
            return gen.characterCountVisible;
        }

        private void SetDrawRangeToContainCaretPosition(int caretPos)
        {
            // We don't have any generated lines generation is not valid.
            if (cachedInputTextGenerator.lineCount <= 0)
                return;

            // the extents gets modified by the pixel density, so we need to use the generated extents since that will be in the same 'space' as
            // the values returned by the TextGenerator.lines[x].height for instance.
            Vector2 extents = cachedInputTextGenerator.rectExtents.size;

            if (multiLine)
            {
                var lines = cachedInputTextGenerator.lines;
                int caretLine = DetermineCharacterLine(caretPos, cachedInputTextGenerator);

                if (caretPos > m_DrawEnd)
                {
                    // Caret comes after drawEnd, so we need to move drawEnd to the end of the line with the caret
                    m_DrawEnd = GetLineEndPosition(cachedInputTextGenerator, caretLine);
                    float bottomY = lines[caretLine].topY - lines[caretLine].height;
                    if (caretLine == lines.Count - 1)
                    {
                        // Remove interline spacing on last line.
                        bottomY += lines[caretLine].leading;
                    }
                    int startLine = caretLine;
                    while (startLine > 0)
                    {
                        float topY = lines[startLine - 1].topY;
                        if (topY - bottomY > extents.y)
                            break;
                        startLine--;
                    }
                    m_DrawStart = GetLineStartPosition(cachedInputTextGenerator, startLine);
                }
                else
                {
                    if (caretPos < m_DrawStart)
                    {
                        // Caret comes before drawStart, so we need to move drawStart to an earlier line start that comes before caret.
                        m_DrawStart = GetLineStartPosition(cachedInputTextGenerator, caretLine);
                    }

                    int startLine = DetermineCharacterLine(m_DrawStart, cachedInputTextGenerator);
                    int endLine = startLine;

                    float topY = lines[startLine].topY;
                    float bottomY = lines[endLine].topY - lines[endLine].height;

                    if (endLine == lines.Count - 1)
                    {
                        // Remove interline spacing on last line.
                        bottomY += lines[endLine].leading;
                    }

                    while (endLine < lines.Count - 1)
                    {
                        bottomY = lines[endLine + 1].topY - lines[endLine + 1].height;

                        if (endLine + 1 == lines.Count - 1)
                        {
                            // Remove interline spacing on last line.
                            bottomY += lines[endLine + 1].leading;
                        }

                        if (topY - bottomY > extents.y)
                            break;
                        ++endLine;
                    }

                    m_DrawEnd = GetLineEndPosition(cachedInputTextGenerator, endLine);

                    while (startLine > 0)
                    {
                        topY = lines[startLine - 1].topY;
                        if (topY - bottomY > extents.y)
                            break;
                        startLine--;
                    }
                    m_DrawStart = GetLineStartPosition(cachedInputTextGenerator, startLine);
                }
            }
            else
            {
                var characters = cachedInputTextGenerator.characters;
                if (m_DrawEnd > cachedInputTextGenerator.characterCountVisible)
                    m_DrawEnd = cachedInputTextGenerator.characterCountVisible;

                float width = 0.0f;
                if (caretPos > m_DrawEnd || (caretPos == m_DrawEnd && m_DrawStart > 0))
                {
                    // fit characters from the caretPos leftward
                    m_DrawEnd = caretPos;
                    for (m_DrawStart = m_DrawEnd - 1; m_DrawStart >= 0; --m_DrawStart)
                    {
                        if (width + characters[m_DrawStart].charWidth > extents.x)
                            break;

                        width += characters[m_DrawStart].charWidth;
                    }
                    ++m_DrawStart;  // move right one to the last character we could fit on the left
                }
                else
                {
                    if (caretPos < m_DrawStart)
                        m_DrawStart = caretPos;

                    m_DrawEnd = m_DrawStart;
                }

                // fit characters rightward
                for (; m_DrawEnd < cachedInputTextGenerator.characterCountVisible; ++m_DrawEnd)
                {
                    width += characters[m_DrawEnd].charWidth;
                    if (width > extents.x)
                        break;
                }
            }
        }

        /// <summary>
        /// Force the label to update immediatly. This will recalculate the positioning of the caret and the visible text.
        /// </summary>
        public void ForceLabelUpdate()
        {
            UpdateLabel();
        }

        private void MarkGeometryAsDirty()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject))
                return;
#endif

            CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
        }

        /// <summary>
        /// Rebuild the input fields geometry. (caret and highlight).
        /// </summary>
        /// <param name="update">Which update loop we are in.</param>
        public virtual void Rebuild(CanvasUpdate update)
        {
            switch (update)
            {
                case CanvasUpdate.LatePreRender:
                    UpdateGeometry();
                    break;
            }
        }

        /// <summary>
        /// See ICanvasElement.LayoutComplete. Does nothing by default.
        /// </summary>
        public virtual void LayoutComplete()
        { }

        /// <summary>
        /// See ICanvasElement.GraphicUpdateComplete. Does nothing by default.
        /// </summary>
        public virtual void GraphicUpdateComplete()
        { }

        private void UpdateGeometry()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
            // No need to draw a cursor on mobile as its handled by the devices keyboard.
            if (!InPlaceEditing() && !shouldHideMobileInput)
                return;

            if (m_CachedInputRenderer == null && m_TextComponent != null)
            {
                GameObject go = new GameObject(transform.name + " Input Caret", typeof(RectTransform), typeof(CanvasRenderer));
                go.hideFlags = HideFlags.DontSave;
                go.transform.SetParent(m_TextComponent.transform.parent);
                go.transform.SetAsFirstSibling();
                go.layer = gameObject.layer;

                caretRectTrans = go.GetComponent<RectTransform>();
                m_CachedInputRenderer = go.GetComponent<CanvasRenderer>();
                m_CachedInputRenderer.SetMaterial(m_TextComponent.GetModifiedMaterial(Graphic.defaultGraphicMaterial), Texture2D.whiteTexture);

                // Needed as if any layout is present we want the caret to always be the same as the text area.
                go.AddComponent<LayoutElement>().ignoreLayout = true;

                AssignPositioningIfNeeded();
            }

            if (m_CachedInputRenderer == null)
                return;

            OnFillVBO(mesh);
            m_CachedInputRenderer.SetMesh(mesh);
        }

        private void AssignPositioningIfNeeded()
        {
            if (m_TextComponent != null && caretRectTrans != null &&
                (caretRectTrans.localPosition != m_TextComponent.rectTransform.localPosition ||
                 caretRectTrans.localRotation != m_TextComponent.rectTransform.localRotation ||
                 caretRectTrans.localScale != m_TextComponent.rectTransform.localScale ||
                 caretRectTrans.anchorMin != m_TextComponent.rectTransform.anchorMin ||
                 caretRectTrans.anchorMax != m_TextComponent.rectTransform.anchorMax ||
                 caretRectTrans.anchoredPosition != m_TextComponent.rectTransform.anchoredPosition ||
                 caretRectTrans.sizeDelta != m_TextComponent.rectTransform.sizeDelta ||
                 caretRectTrans.pivot != m_TextComponent.rectTransform.pivot))
            {
                caretRectTrans.localPosition = m_TextComponent.rectTransform.localPosition;
                caretRectTrans.localRotation = m_TextComponent.rectTransform.localRotation;
                caretRectTrans.localScale = m_TextComponent.rectTransform.localScale;
                caretRectTrans.anchorMin = m_TextComponent.rectTransform.anchorMin;
                caretRectTrans.anchorMax = m_TextComponent.rectTransform.anchorMax;
                caretRectTrans.anchoredPosition = m_TextComponent.rectTransform.anchoredPosition;
                caretRectTrans.sizeDelta = m_TextComponent.rectTransform.sizeDelta;
                caretRectTrans.pivot = m_TextComponent.rectTransform.pivot;
            }
        }

        private void OnFillVBO(Mesh vbo)
        {
            using (var helper = new VertexHelper())
            {
                if (!isFocused)
                {
                    helper.FillMesh(vbo);
                    return;
                }

                Vector2 roundingOffset = m_TextComponent.PixelAdjustPoint(Vector2.zero);
                if (!hasSelection)
                    GenerateCaret(helper, roundingOffset);
                else
                    GenerateHighlight(helper, roundingOffset);

                helper.FillMesh(vbo);
            }
        }

        private void GenerateCaret(VertexHelper vbo, Vector2 roundingOffset)
        {
            if (!m_CaretVisible)
                return;

            if (m_CursorVerts == null)
            {
                CreateCursorVerts();
            }

            float width = m_CaretWidth;
            int adjustedPos = Mathf.Max(0, caretPositionInternal - m_DrawStart);
            TextGenerator gen = m_TextComponent.cachedTextGenerator;

            if (gen == null)
                return;

            if (gen.lineCount == 0)
                return;

            Vector2 startPosition = Vector2.zero;

            // Calculate startPosition
            if (adjustedPos < gen.characters.Count)
            {
                UICharInfo cursorChar = gen.characters[adjustedPos];
                startPosition.x = cursorChar.cursorPos.x;
            }
            startPosition.x /= m_TextComponent.pixelsPerUnit;

            // TODO: Only clamp when Text uses horizontal word wrap.
            if (startPosition.x > m_TextComponent.rectTransform.rect.xMax)
                startPosition.x = m_TextComponent.rectTransform.rect.xMax;

            int characterLine = DetermineCharacterLine(adjustedPos, gen);
            startPosition.y = gen.lines[characterLine].topY / m_TextComponent.pixelsPerUnit;
            float height = gen.lines[characterLine].height / m_TextComponent.pixelsPerUnit;

            for (int i = 0; i < m_CursorVerts.Length; i++)
                m_CursorVerts[i].color = caretColor;

            m_CursorVerts[0].position = new Vector3(startPosition.x, startPosition.y - height, 0.0f);
            m_CursorVerts[1].position = new Vector3(startPosition.x + width, startPosition.y - height, 0.0f);
            m_CursorVerts[2].position = new Vector3(startPosition.x + width, startPosition.y, 0.0f);
            m_CursorVerts[3].position = new Vector3(startPosition.x, startPosition.y, 0.0f);

            if (roundingOffset != Vector2.zero)
            {
                for (int i = 0; i < m_CursorVerts.Length; i++)
                {
                    UIVertex uiv = m_CursorVerts[i];
                    uiv.position.x += roundingOffset.x;
                    uiv.position.y += roundingOffset.y;
                }
            }

            vbo.AddUIVertexQuad(m_CursorVerts);

            int screenHeight = Screen.height;
            // Multiple display support only when not the main display. For display 0 the reported
            // resolution is always the desktops resolution since its part of the display API,
            // so we use the standard none multiple display method. (case 741751)
            int displayIndex = m_TextComponent.canvas.targetDisplay;
            if (displayIndex > 0 && displayIndex < Display.displays.Length)
                screenHeight = Display.displays[displayIndex].renderingHeight;

            // Calculate position of IME Window in screen space.
            Camera cameraRef;
            if (m_TextComponent.canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                cameraRef = null;
            else
                cameraRef = m_TextComponent.canvas.worldCamera;

            Vector3 cursorPosition = m_CachedInputRenderer.gameObject.transform.TransformPoint(m_CursorVerts[0].position);
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(cameraRef, cursorPosition);
            screenPosition.y = screenHeight - screenPosition.y;

            if (input != null)
                input.compositionCursorPos = screenPosition;
        }

        private void CreateCursorVerts()
        {
            m_CursorVerts = new UIVertex[4];

            for (int i = 0; i < m_CursorVerts.Length; i++)
            {
                m_CursorVerts[i] = UIVertex.simpleVert;
                m_CursorVerts[i].uv0 = Vector2.zero;
            }
        }

        private void GenerateHighlight(VertexHelper vbo, Vector2 roundingOffset)
        {
            int startChar = Mathf.Max(0, caretPositionInternal - m_DrawStart);
            int endChar = Mathf.Max(0, caretSelectPositionInternal - m_DrawStart);

            // Ensure pos is always less then selPos to make the code simpler
            if (startChar > endChar)
            {
                int temp = startChar;
                startChar = endChar;
                endChar = temp;
            }

            endChar -= 1;
            TextGenerator gen = m_TextComponent.cachedTextGenerator;

            if (gen.lineCount <= 0)
                return;

            int currentLineIndex = DetermineCharacterLine(startChar, gen);

            int lastCharInLineIndex = GetLineEndPosition(gen, currentLineIndex);

            UIVertex vert = UIVertex.simpleVert;
            vert.uv0 = Vector2.zero;
            vert.color = selectionColor;

            int currentChar = startChar;
            while (currentChar <= endChar && currentChar < gen.characterCount)
            {
                if (currentChar == lastCharInLineIndex || currentChar == endChar)
                {
                    UICharInfo startCharInfo = gen.characters[startChar];
                    UICharInfo endCharInfo = gen.characters[currentChar];
                    Vector2 startPosition = new Vector2(startCharInfo.cursorPos.x / m_TextComponent.pixelsPerUnit, gen.lines[currentLineIndex].topY / m_TextComponent.pixelsPerUnit);
                    Vector2 endPosition = new Vector2((endCharInfo.cursorPos.x + endCharInfo.charWidth) / m_TextComponent.pixelsPerUnit, startPosition.y - gen.lines[currentLineIndex].height / m_TextComponent.pixelsPerUnit);

                    // Checking xMin as well due to text generator not setting position if char is not rendered.
                    if (endPosition.x > m_TextComponent.rectTransform.rect.xMax || endPosition.x < m_TextComponent.rectTransform.rect.xMin)
                        endPosition.x = m_TextComponent.rectTransform.rect.xMax;

                    var startIndex = vbo.currentVertCount;
                    vert.position = new Vector3(startPosition.x, endPosition.y, 0.0f) + (Vector3)roundingOffset;
                    vbo.AddVert(vert);

                    vert.position = new Vector3(endPosition.x, endPosition.y, 0.0f) + (Vector3)roundingOffset;
                    vbo.AddVert(vert);

                    vert.position = new Vector3(endPosition.x, startPosition.y, 0.0f) + (Vector3)roundingOffset;
                    vbo.AddVert(vert);

                    vert.position = new Vector3(startPosition.x, startPosition.y, 0.0f) + (Vector3)roundingOffset;
                    vbo.AddVert(vert);

                    vbo.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
                    vbo.AddTriangle(startIndex + 2, startIndex + 3, startIndex + 0);

                    startChar = currentChar + 1;
                    currentLineIndex++;

                    lastCharInLineIndex = GetLineEndPosition(gen, currentLineIndex);
                }
                currentChar++;
            }
        }

        /// <summary>
        /// Predefined validation functionality for different characterValidation types.
        /// </summary>
        /// <param name="text">The whole text string to validate.</param>
        /// <param name="pos">The position at which the current character is being inserted.</param>
        /// <param name="ch">The character that is being inserted</param>
        /// <returns>The character that should be inserted.</returns>
        protected char Validate(string text, int pos, char ch)
        {
            // Validation is disabled
            if (characterValidation == CharacterValidation.None || !enabled)
                return ch;

            if (characterValidation == CharacterValidation.Integer || characterValidation == CharacterValidation.Decimal)
            {
                // Integer and decimal
                bool cursorBeforeDash = (pos == 0 && text.Length > 0 && text[0] == '-');
                bool dashInSelection = text.Length > 0 && text[0] == '-' && ((caretPositionInternal == 0 && caretSelectPositionInternal > 0) || (caretSelectPositionInternal == 0 && caretPositionInternal > 0));
                bool selectionAtStart = caretPositionInternal == 0 || caretSelectPositionInternal == 0;
                if (!cursorBeforeDash || dashInSelection)
                {
                    if (ch >= '0' && ch <= '9') return ch;
                    if (ch == '-' && (pos == 0 || selectionAtStart)) return ch;
                    if ((ch == '.' || ch == ',') && characterValidation == CharacterValidation.Decimal && text.IndexOfAny(new[] { '.', ',' }) == -1) return ch;
                }
            }
            else if (characterValidation == CharacterValidation.Alphanumeric)
            {
                // All alphanumeric characters
                if (ch >= 'A' && ch <= 'Z') return ch;
                if (ch >= 'a' && ch <= 'z') return ch;
                if (ch >= '0' && ch <= '9') return ch;
            }
            else if (characterValidation == CharacterValidation.Name)
            {
                // FIXME: some actions still lead to invalid input:
                //        - Hitting delete in front of an uppercase letter
                //        - Selecting an uppercase letter and deleting it
                //        - Typing some text, hitting Home and typing more text (we then have an uppercase letter in the middle of a word)
                //        - Typing some text, hitting Home and typing a space (we then have a leading space)
                //        - Erasing a space between two words (we then have an uppercase letter in the middle of a word)
                //        - We accept a trailing space
                //        - We accept the insertion of a space between two lowercase letters.
                //        - Typing text in front of an existing uppercase letter
                //        - ... and certainly more
                //
                // The rule we try to implement are too complex for this kind of verification.

                if (char.IsLetter(ch))
                {
                    // Character following a space or a hyphen should be in uppercase.
                    if (char.IsLower(ch) && ((pos == 0) || (text[pos - 1] == ' ') || (text[pos - 1] == '-')))
                    {
                        return char.ToUpper(ch);
                    }

                    // Character not following a space or an apostrophe or a hyphen should be in lowercase.
                    if (char.IsUpper(ch) && (pos > 0) && (text[pos - 1] != ' ') && (text[pos - 1] != '\'') && (text[pos - 1] != '-'))
                    {
                        return char.ToLower(ch);
                    }

                    return ch;
                }

                if (ch == '\'')
                {
                    // Don't allow more than one apostrophe
                    if (!text.Contains("'"))
                        // Don't allow consecutive spaces and apostrophes.
                        if (!(((pos > 0) && ((text[pos - 1] == ' ') || (text[pos - 1] == '\'') || (text[pos - 1] == '-'))) ||
                              ((pos < text.Length) && ((text[pos] == ' ') || (text[pos] == '\'') || (text[pos] == '-')))))
                            return ch;
                }

                if (ch == ' ' || ch == '-')
                {
                    if (pos != 0) // Don't allow leading spaces and hyphens
                    {
                        // Don't allow consecutive spaces, apostrophes and hyphens.
                        if (!(((pos > 0) && ((text[pos - 1] == ' ') || (text[pos - 1] == '\'') || (text[pos - 1] == '-'))) ||
                              ((pos < text.Length) && ((text[pos] == ' ') || (text[pos] == '\'') || (text[pos - 1] == '-')))))
                            return ch;
                    }
                }
            }
            else if (characterValidation == CharacterValidation.EmailAddress)
            {
                // From StackOverflow about allowed characters in email addresses:
                // Uppercase and lowercase English letters (a-z, A-Z)
                // Digits 0 to 9
                // Characters ! # $ % & ' * + - / = ? ^ _ ` { | } ~
                // Character . (dot, period, full stop) provided that it is not the first or last character,
                // and provided also that it does not appear two or more times consecutively.

                if (ch >= 'A' && ch <= 'Z') return ch;
                if (ch >= 'a' && ch <= 'z') return ch;
                if (ch >= '0' && ch <= '9') return ch;
                if (ch == '@' && text.IndexOf('@') == -1) return ch;
                if (kEmailSpecialCharacters.IndexOf(ch) != -1) return ch;
                if (ch == '.')
                {
                    char lastChar = (text.Length > 0) ? text[Mathf.Clamp(pos, 0, text.Length - 1)] : ' ';
                    char nextChar = (text.Length > 0) ? text[Mathf.Clamp(pos + 1, 0, text.Length - 1)] : '\n';
                    if (lastChar != '.' && nextChar != '.')
                        return ch;
                }
            }
            return (char)0;
        }
        public void ActivateInputField()
        {
            if (m_TextComponent == null || m_TextComponent.font == null || !IsActive() || !IsInteractable())
                return;

            if (isFocused)
            {
                if (m_Keyboard != null && !m_Keyboard.active)
                {
                    m_Keyboard.active = true;
                    m_Keyboard.text = m_Text;
                }
            }

            m_ShouldActivateNextUpdate = true;
        }

        private void ActivateInputFieldInternal()
        {
            if (EventSystem.current == null)
                return;

            if (EventSystem.current.currentSelectedGameObject != gameObject)
                EventSystem.current.SetSelectedGameObject(gameObject);

            // Cache the value of isInPlaceEditingAllowed, because on UWP this involves calling into native code
            // Usually, the value only needs to be updated once when the TouchKeyboard is opened; however, on Chrome OS,
            // we check repeatedly to see if the in-place editing state has changed, so we can take action.
            m_TouchKeyboardAllowsInPlaceEditing = TouchScreenKeyboard.isInPlaceEditingAllowed;

            if (TouchScreenKeyboardShouldBeUsed())
            {
                if (input != null && input.touchSupported)
                {
                    TouchScreenKeyboard.hideInput = shouldHideMobileInput;
                }
                m_Keyboard = (inputType == InputType.Password) ?
                    TouchScreenKeyboard.Open(m_Text, keyboardType, false, multiLine, true, false, "", characterLimit) :
                    TouchScreenKeyboard.Open(m_Text, keyboardType, inputType == InputType.AutoCorrect, multiLine, false, false, "", characterLimit);

                // If TouchKeyboard doesn't support InPlaceEditing don't call OnFocus as mobile doesn't properly support select all
                // Just set it to the end of the text (where it would move when typing starts)
                if (!m_TouchKeyboardAllowsInPlaceEditing)
                {
                    MoveTextEnd(false);
                }
            }

            // Perform normal OnFocus routine if platform supports it
            if (!TouchScreenKeyboard.isSupported || m_TouchKeyboardAllowsInPlaceEditing)
            {
                if (input != null)
                    input.imeCompositionMode = IMECompositionMode.On;
                OnFocus();
            }
            m_AllowInput = true;
            m_OriginalText = text;
            m_WasCanceled = false;
            SetCaretVisible();
            UpdateLabel();
        }

        /// <summary>
        /// What to do when the event system sends a submit Event.
        /// </summary>
        /// <param name="eventData">The data on which to process</param>
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            if (shouldActivateOnSelect)
                ActivateInputField();
        }

        /// <summary>
        /// What to do when the event system sends a pointer click Event
        /// </summary>
        /// <param name="eventData">The data on which to process</param>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            ActivateInputField();
        }

        public void DeactivateInputField()
        {
            // Not activated do nothing.
            if (!m_AllowInput)
                return;

            m_HasDoneFocusTransition = false;
            m_AllowInput = false;

            if (m_Placeholder != null)
                m_Placeholder.enabled = string.IsNullOrEmpty(m_Text);

            if (m_TextComponent != null && IsInteractable())
            {
                if (m_WasCanceled)
                    text = m_OriginalText;

                SendOnEndEdit();

                if (m_Keyboard != null)
                {
                    m_Keyboard.active = false;
                    m_Keyboard = null;
                }

                m_CaretPosition = m_CaretSelectPosition = 0;
                if (input != null)
                    input.imeCompositionMode = IMECompositionMode.Auto;
            }

            MarkGeometryAsDirty();
        }

        /// <summary>
        /// What to do when the event system sends a Deselect Event. Defaults to deactivating the inputfield.
        /// </summary>
        /// <param name="eventData">The data sent by the EventSystem</param>
        public override void OnDeselect(BaseEventData eventData)
        {
            DeactivateInputField();
            base.OnDeselect(eventData);
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
                return;

            if (!isFocused)
                m_ShouldActivateNextUpdate = true;
        }

        private void EnforceContentType()
        {
            switch (contentType)
            {
                case ContentType.Standard:
                    {
                        // Don't enforce line type for this content type.
                        m_InputType = InputType.Standard;
                        m_KeyboardType = TouchScreenKeyboardType.Default;
                        m_CharacterValidation = CharacterValidation.None;
                        break;
                    }
                case ContentType.Autocorrected:
                    {
                        // Don't enforce line type for this content type.
                        m_InputType = InputType.AutoCorrect;
                        m_KeyboardType = TouchScreenKeyboardType.Default;
                        m_CharacterValidation = CharacterValidation.None;
                        break;
                    }
                case ContentType.IntegerNumber:
                    {
                        m_LineType = LineType.SingleLine;
                        m_InputType = InputType.Standard;
                        m_KeyboardType = TouchScreenKeyboardType.NumberPad;
                        m_CharacterValidation = CharacterValidation.Integer;
                        break;
                    }
                case ContentType.DecimalNumber:
                    {
                        m_LineType = LineType.SingleLine;
                        m_InputType = InputType.Standard;
                        m_KeyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
                        m_CharacterValidation = CharacterValidation.Decimal;
                        break;
                    }
                case ContentType.Alphanumeric:
                    {
                        m_LineType = LineType.SingleLine;
                        m_InputType = InputType.Standard;
                        m_KeyboardType = TouchScreenKeyboardType.ASCIICapable;
                        m_CharacterValidation = CharacterValidation.Alphanumeric;
                        break;
                    }
                case ContentType.Name:
                    {
                        m_LineType = LineType.SingleLine;
                        m_InputType = InputType.Standard;
                        m_KeyboardType = TouchScreenKeyboardType.NamePhonePad;
                        m_CharacterValidation = CharacterValidation.Name;
                        break;
                    }
                case ContentType.EmailAddress:
                    {
                        m_LineType = LineType.SingleLine;
                        m_InputType = InputType.Standard;
                        m_KeyboardType = TouchScreenKeyboardType.EmailAddress;
                        m_CharacterValidation = CharacterValidation.EmailAddress;
                        break;
                    }
                case ContentType.Password:
                    {
                        m_LineType = LineType.SingleLine;
                        m_InputType = InputType.Password;
                        m_KeyboardType = TouchScreenKeyboardType.Default;
                        m_CharacterValidation = CharacterValidation.None;
                        break;
                    }
                case ContentType.Pin:
                    {
                        m_LineType = LineType.SingleLine;
                        m_InputType = InputType.Password;
                        m_KeyboardType = TouchScreenKeyboardType.NumberPad;
                        m_CharacterValidation = CharacterValidation.Integer;
                        break;
                    }
                default:
                    {
                        // Includes Custom type. Nothing should be enforced.
                        break;
                    }
            }

            EnforceTextHOverflow();
        }

        void EnforceTextHOverflow()
        {
            if (m_TextComponent != null)
                if (multiLine)
                    m_TextComponent.horizontalOverflow = HorizontalWrapMode.Wrap;
                else
                    m_TextComponent.horizontalOverflow = HorizontalWrapMode.Overflow;
        }

        void SetToCustomIfContentTypeIsNot(params ContentType[] allowedContentTypes)
        {
            if (contentType == ContentType.Custom)
                return;

            for (int i = 0; i < allowedContentTypes.Length; i++)
                if (contentType == allowedContentTypes[i])
                    return;

            contentType = ContentType.Custom;
        }

        void SetToCustom()
        {
            if (contentType == ContentType.Custom)
                return;

            contentType = ContentType.Custom;
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (m_HasDoneFocusTransition)
                state = SelectionState.Selected;
            else if (state == SelectionState.Pressed)
                m_HasDoneFocusTransition = true;

            base.DoStateTransition(state, instant);
        }

        public virtual void CalculateLayoutInputHorizontal() { }

        public virtual void CalculateLayoutInputVertical() { }

        public virtual float minWidth { get { return 0; } }

        public virtual float preferredWidth
        {
            get
            {
                if (textComponent == null)
                    return 0;
                var settings = textComponent.GetGenerationSettings(Vector2.zero);
                return textComponent.cachedTextGeneratorForLayout.GetPreferredWidth(m_Text, settings) / textComponent.pixelsPerUnit;
            }
        }
        public virtual float flexibleWidth { get { return -1; } }

        public virtual float minHeight { get { return 0; } }

        public virtual float preferredHeight
        {
            get
            {
                if (textComponent == null)
                    return 0;
                var settings = textComponent.GetGenerationSettings(new Vector2(textComponent.rectTransform.rect.size.x, 0.0f));
                return textComponent.cachedTextGeneratorForLayout.GetPreferredHeight(m_Text, settings) / textComponent.pixelsPerUnit;
            }
        }

        public virtual float flexibleHeight { get { return -1; } }

        public virtual int layoutPriority { get { return 1; } }
    }
}

