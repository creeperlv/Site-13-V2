using System;
using UnityEngine;

namespace Site13Kernel.UI
{
    [Serializable]
    public class ComboBoxData
    {
        [SerializeField]
        private string m_Text;
        [SerializeField]
        private Sprite m_Image;

        public string text { get { return m_Text; } set { m_Text = value; } }

        public Sprite image { get { return m_Image; } set { m_Image = value; } }

        public ComboBoxData()
        {
        }

        public ComboBoxData(string text)
        {
            this.text = text;
        }

        public ComboBoxData(Sprite image)
        {
            this.image = image;
        }

        public ComboBoxData(string text, Sprite image)
        {
            this.text = text;
            this.image = image;
        }
    }
}
