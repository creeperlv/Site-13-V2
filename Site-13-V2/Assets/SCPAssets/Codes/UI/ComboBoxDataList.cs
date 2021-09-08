using System;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.UI
{
    [Serializable]
    public class ComboBoxDataList
    {
        [SerializeField]
        private List<ComboBoxData> m_Options;

        public List<ComboBoxData> options { get { return m_Options; } set { m_Options = value; } }


        public ComboBoxDataList()
        {
            options = new List<ComboBoxData>();
        }
    }
}
