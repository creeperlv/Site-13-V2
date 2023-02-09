using Site13Kernel.Attributes;
using Site13Kernel.UI.xUI;
using Site13Kernel.xUIImpl;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using xUI.Core.Abstraction;

namespace Site13Kernel.UI.xUI
{
    public class xUIWindowImpl : xUIElementImplBase,IWindowImpl
    {
        public RectTransform ControlledTransform;
        public xUIResizableBorder Border;
        public Vector2 MinSize = new Vector2(300, 50);
        public xUIPointerDownImpl Focus;
        public xUIButtonImpl CloseBtn;
        public xUIButtonImpl MinimizeBtn;
        public xUIButtonImpl MaximizeBtn;
        public Transform OriginalBackground;
        public Transform CustomizedBackground;
        public Transform Content;
        public Text TitleText;
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
            //Focus.onPointerDown=() => { WindowManager.Focus(this); };
        }
        public void LossFocus()
        {
            Focus.gameObject.SetActive(true);
        }
        public void GainFocus()
        {
            Focus.gameObject.SetActive(false);
        }

        public void SetTitle(string title)
        {
            TitleText.text = title;
        }

        public void SetIcon(object obj)
        {
            throw new System.NotImplementedException();
        }

        public void SetWindowMode(WindowMode wm)
        {
            switch (wm)
            {
                case WindowMode.Full:
                    {
                        CloseBtn.gameObject.SetActive(true);
                        MinimizeBtn.gameObject.SetActive(true);
                        MaximizeBtn.gameObject.SetActive(true);
                        TitleText.gameObject.SetActive(true);
                    }
                    break;
                case WindowMode.NoBorder:

                    {
                        CloseBtn.gameObject.SetActive(false);
                        MinimizeBtn.gameObject.SetActive(false);
                        MaximizeBtn.gameObject.SetActive(false);
                        TitleText.gameObject.SetActive(false);
                    }
                    break;
                case WindowMode.NoTitle:
                    {
                        CloseBtn.gameObject.SetActive(true);
                        MinimizeBtn.gameObject.SetActive(true);
                        MaximizeBtn.gameObject.SetActive(true);
                        TitleText.gameObject.SetActive(false);
                    }
                    break;
                case WindowMode.Minimal:
                    {
                        CloseBtn.gameObject.SetActive(true);
                        MinimizeBtn.gameObject.SetActive(true);
                        MaximizeBtn.gameObject.SetActive(false);
                        TitleText.gameObject.SetActive(false);

                    }
                    break;
                default:
                    break;
            }
        }

        public void Close()
        {
            CloseBtn.OnClick();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }
        [TODO]
        public void Hide()
        {
            throw new System.NotImplementedException();
        }

        [TODO]
        public void DisableDefaultWindowBackground()
        {
            throw new System.NotImplementedException();
        }

        [TODO]
        public void SetMenuBar(IMenu menu)
        {
            throw new System.NotImplementedException();
        }

        public void SetExtendContentToTitleBar(bool value)
        {
            throw new System.NotImplementedException();
        }

        public void SetBackButton(bool Show)
        {
            throw new System.NotImplementedException();
        }
    }
}
