using Site13Kernel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.UI
{
    public class DialogManager : ControlledBehavior
    {
        static DialogManager Current;
        public GameObject DialogTemplate_OK;
        public GameObject DialogTemplate_OK_Cancel;
        public List<Dialog> ManagedDialogs=new List<Dialog>();
        public override void Init()
        {
            Current = this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _Show(string Title, string Content, string Option1, Action Option1Act, Sprite Icon = null)
        {
            var D = GameObject.Instantiate(DialogTemplate_OK, this.transform);
            //ManagedDialogs.Add();
            var DIA = D.GetComponent<Dialog>();
            DIA.Initialize(Title, Content, Option1, Option1Act, null, null);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _Show(string Title, string Content, string Option1, Action Option1Act, string Option2, Action Option2Act, Sprite Icon = null)
        {
            var D = GameObject.Instantiate(DialogTemplate_OK, this.transform);
            //ManagedDialogs.Add();
            var DIA = D.GetComponent<Dialog>();
            DIA.Initialize(Title, Content, Option1, Option1Act, Option2, Option2Act);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Show(string Title, string Content, string Option1, Action Option1Act, Sprite Icon = null)
        {
            Current._Show(Title, Content, Option1, Option1Act, Icon);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Show(string Title, string Content, string Option1, Action Option1Act, string Option2, Action Option2Act, Sprite Icon = null)
        {
            Current._Show(Title, Content, Option1, Option1Act, Option2,Option2Act,Icon);
        }
    }
}
