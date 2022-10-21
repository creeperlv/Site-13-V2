using Site13Kernel.Core;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic
{
    public class CrosshairContainer : ControlledBehavior
    {
        public CanvasGroup canvas;
        public GameObject Contaienr;
        public List<ControlledCrosshair> Children = new List<ControlledCrosshair>();
        public override void Init()
        {
            foreach (var c in Children)
            {
                c.Init();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Show()
        {
            if (Contaienr != null)
            {
                Contaienr.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(true);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Hide()
        {
            if (Contaienr != null)
            {
                Contaienr.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateCrosshair(float Recoil)
        {
            foreach (var crosshair in Children)
            {
                crosshair.UpdateCrosshair(Recoil);
            }
        }
    }
}
