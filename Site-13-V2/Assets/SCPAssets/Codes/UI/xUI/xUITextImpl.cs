using Site13Kernel.UI.xUI.uUIImplementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using xUI.Core.Abstraction;
using xUI.Core.UIElements;

namespace Site13Kernel
{
    public class xUITextImpl : xUIElementImplBase, IContentImpl, IxUITextableImpl
    {
        public bool isTMP;
        public Text uUIText;
        public xUIText ori;
        public override void Bind(IUIElement element)
        {
            if (element is xUIText text)
            {
                ori = text;
            }
        }

        public override void Repaint()
        {

        }

        public void SetContent(object content)
        {
            if (content is string str)
            {
                if (isTMP)
                {

                }
                else
                {
                    uUIText.text = str;

                }
            }
        }

        public void SetFontFamily(string s)
        {
            if (isTMP) { }
            else
            {
                uUIText.font = uUIRendererResources.Instance.FontsDictionary[s];
            }
        }

        public void SetFontSize(int s)
        {
            if (isTMP)
            {

            }
            else
            {
                uUIText.fontSize = s;
            }
        }

        public override void SetHit(bool IsEnabled)
        {
            if (isTMP)
            {

            }
            else
            {
                uUIText.raycastTarget = IsEnabled;
            }
        }

        public override void SetIsEnable(bool State)
        {
            if (isTMP)
            {

            }
            else
            {
                uUIText.gameObject.SetActive(State);
            }
        }
    }
}
