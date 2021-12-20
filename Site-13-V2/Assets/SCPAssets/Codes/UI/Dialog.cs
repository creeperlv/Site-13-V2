using Site13Kernel.Core;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI
{
    public class Dialog : ControlledBehavior
    {
        public Text Header;
        public Text Content;
        public Image Icon;
        public UIButton OKButton;
        public UIButton CancelButton;
        public Action OKAction;
        public Action CancelAction;
        public float CloseAnimeLength;
        public Animator Animator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Initialize(string Title, string Content, string Option1, Action Option1Act, string Option2, Action Option2Act, Sprite Icon = null)
        {
            OKAction = Option1Act;
            CancelAction = Option2Act;
            Header.text = Title;
            this.Content.text = Content;
            if (Option1 != null)
            {
                OKButton.Content = Option1;
                OKButton.OnClick = () =>
                 {
                     if (OKAction != null) OKAction();
                     CloseDialog();
                 };
            }
            else
            {
                OKButton.gameObject.SetActive(false);
            }
            if (Option2 != null)
            {
                CancelButton.Content = Option2;
                CancelButton.OnClick = () =>
                {
                    if (CancelAction != null) CancelAction();
                    CloseDialog();
                };
            }
            else
            {
                CancelButton.gameObject.SetActive(false);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CloseDialog()
        {
            StartCoroutine(RealClose());
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator RealClose()
        {
            Animator.SetTrigger("Close");
            yield return new WaitForSeconds(CloseAnimeLength);
            GameObject.Destroy(this.gameObject);
        }
    }
}
