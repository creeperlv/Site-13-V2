using CLUNL.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI
{
    public class PostGameCarnageReportItem : MonoBehaviour
    {
        public Image Icon;
        public Text Description;
        public Text Count;
        public void SetData(LocalizedString Desc,int Count,Sprite Icon=null)
        {
            if (Icon != null)
            {
                this.Icon.sprite = Icon;
            }
            else
            {
                this.Icon.gameObject.SetActive(false);
            }
            Description.text = Desc;
            this.Count.text = Count + "";
        }
    }
}
