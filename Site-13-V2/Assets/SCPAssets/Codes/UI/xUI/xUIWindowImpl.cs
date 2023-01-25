using Site13Kernel.UI.xUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.UI.xUI
{
    public class xUIWindowImpl : MonoBehaviour
    {
        public RectTransform ControlledTransform;
        public xUIResizableBorder Border;
        public Vector2 MinSize = new Vector2(300, 50);
        public xUIPointerDownImpl Focus;
        void Start()
        {
            WindowManager.RegisterWindow(this);
            Border.OnSizeChange = (v) => {
                if (v.y < MinSize.y)
                {
                    var s=ControlledTransform.sizeDelta;
                    s.y = MinSize.y;
                    ControlledTransform.sizeDelta = s;
                }
                if (v.x < MinSize.x)
                {
                    var s = ControlledTransform.sizeDelta;
                    s.x = MinSize.x;
                    ControlledTransform.sizeDelta = s;
                }
            };
            Focus.onPointerDown=() => { WindowManager.Focus(this); };
        }
        public void LossFocus()
        {
            Focus.gameObject.SetActive(true);
        }
        public void GainFocus()
        {
            Focus.gameObject.SetActive(false);
        }
    }
    public static class WindowManager
    {
        public static List<xUIWindowImpl> windows=new List<xUIWindowImpl>();
        public static void RegisterWindow(xUIWindowImpl windowImpl)
        {
            windows.Add(windowImpl);
        }
        public static void Focus(xUIWindowImpl windowImpl)
        {
            windowImpl.GainFocus();
            windowImpl.transform.SetAsLastSibling();
            foreach (var item in windows)
            {
                if (item != windowImpl)
                {
                    item.LossFocus();
                }
            }
        }
    }
}
