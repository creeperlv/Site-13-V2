using CLUNL.Localization;
using Site13Kernel.Core;
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
        public bool AllowPickupAmmo;
        public static ActiveInteractor Instance;
        public bool InteractorEnabled = true;
        public InteractiveBase Interactive = null;
        public bool InputControlled = false;
        public LocalizedString Hint;
        public BipedEntity CurrentEntity;
        public bool __hint = false;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnInvoke(InteractiveBase Interactive)
        {
            if (Interactive.isOperating)
                Interactive.UnOperate();
            Interactive.isOperating = false;

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnInteract()
        {
            if (Interactive != null)
                UnInvoke(Interactive);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Interact()
        {
            IInvoke(Interactive, 0, 0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IInvoke(InteractiveBase Interactive, float DeltaTime, float UnscaledDeltaTime)
        {
            if (Interactive.OperationMode == OperationMode.SingleFrame)
            {
                if (Interactive.isOperating != true)
                {
                    Interactive.Operate(DeltaTime, UnscaledDeltaTime, CurrentEntity);
                    Interactive.isOperating = true;
                }
            }
            else
            {

                Interactive.Operate(DeltaTime, UnscaledDeltaTime, CurrentEntity);
                if (Interactive.isOperating != true)
                {
                    Interactive.isOperating = true;
                }
            }
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
                __hint = true;
                Hint = new LocalizedString(Interactive.OperateHint, Interactive.OperateHintFallBack);
                //if (InteractHint != null)
                //{
                //    InteractHint.Visibility = true;
                //    InteractHint.Content = Language.Find(Interactive.OperateHint, Interactive.OperateHintFallBack);
                //}
            }
            else
            {
                __hint = true;
                Hint = new LocalizedString(string.Empty, string.Empty);
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
                    p.ObtainRemaining(this.CurrentEntity);
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
