using CLUNL.Localization;
using Site13Kernel.Core.Interactives;
using Site13Kernel.Core.Profiles;
using Site13Kernel.GameLogic.FPS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.Controls
{
    public class ActiveInteractor : MonoBehaviour
    {
#if UNITY_EDITOR
        public string ID;
        public Profile Profile;
#endif
        public Guid PlayerID;
        public static ActiveInteractor Instance;
        public bool InteractorEnabled = true;
        public bool InputControlled = false;
        public void OnEnable()
        {
            if (InputControlled)
                Instance = this;
#if UNITY_EDITOR
            ID = PlayerID.ToString();
#endif
        }
        
        public void ActiveInteract()
        {

        }
        float InteractTime;
        InteractiveBase Interactive = null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnInvoke(InteractiveBase Interactive)
        {
            if (Interactive.isOperating)
                Interactive.UnOperate();
            Interactive.isOperating = false;

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SwapInteractive(InteractiveBase _Interactive)
        {
            if (_Interactive == null)
                if (Interactive != null)
                {
                    if (Interactive.isCollision)
                    {
                        return;
                    }
                }
            if (Interactive != _Interactive)
            {
                if (Interactive != null)
                {
                    UnInvoke(Interactive);
                }
            }
            Interactive = _Interactive;
            if (Interactive != null)
            {
                //if (InteractHint != null)
                //{
                //    InteractHint.Visibility = true;
                //    InteractHint.Content = Language.Find(Interactive.OperateHint, Interactive.OperateHintFallBack);
                //}
            }
            else
            {
                //if (InteractHint != null)
                //{
                //    InteractHint.Visibility = false;
                //}
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            var interactive = other.gameObject.GetComponent<InteractiveBase>();
            if (interactive != null)
            {
                interactive.isCollision = true;
                SwapInteractive(interactive);
                if (interactive is Pickupable p)
                {
                    //p.ObtainRemaining(BagHolder);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            var interactive = other.gameObject.GetComponent<InteractiveBase>();
            if (interactive != null)
            {
                if (interactive.isCollision && interactive == Interactive)
                {
                    Interactive.isCollision = false;
                    UnInvoke(Interactive);
                    SwapInteractive(null);
                }
            }
        }
    }
}
