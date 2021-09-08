using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Site13Kernel.UI
{
    public class ComboBoxItem : MonoBehaviour, IPointerEnterHandler, ICancelHandler
    {
        [SerializeField]
        private Text m_Text;
        [SerializeField]
        private Image m_Image;
        [SerializeField]
        private RectTransform m_RectTransform;
        [SerializeField]
        private Toggle m_Toggle;

        public Text text { get { return m_Text; } set { m_Text = value; } }
        public Image image { get { return m_Image; } set { m_Image = value; } }
        public RectTransform rectTransform { get { return m_RectTransform; } set { m_RectTransform = value; } }
        public Toggle toggle { get { return m_Toggle; } set { m_Toggle = value; } }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public virtual void OnCancel(BaseEventData eventData)
        {
            Dropdown dropdown = GetComponentInParent<Dropdown>();
            if (dropdown)
                dropdown.Hide();
        }
    }
}
