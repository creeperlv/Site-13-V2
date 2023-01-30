using System.Collections.Generic;
using UnityEngine;
using xUI.Core.Abstraction;

namespace Site13Kernel.UI.xUI
{
    public class WindowManager: MonoBehaviour,IWindowManager
    {
        public static List<xUIWindowImpl> windows=new List<xUIWindowImpl>();
        private void Start()
        {
            IWindowManager.CurrentManager = this;
        }
        
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

        public void BringWindowToFront(IWindow window)
        {
        }

        public void MakeTopMostWindow(IWindow window)
        {
        }
    }
}
