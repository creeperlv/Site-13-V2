using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.UI.Containers
{
    [Serializable]
    public class FadeNavigatableItem: NavigatableItem
    {
        public CanvasGroup ActualItem;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Hide(float DeltaTime)
        {
            if (ActualItem.alpha > 0)
            {
                ActualItem.alpha -= DeltaTime;

                var d = 1 - ActualItem.alpha;
                d *= -400;
                ActualItem.transform.localPosition = new Vector3(ActualItem.transform.localPosition.x, ActualItem.transform.localPosition.y, d);
            }
            else
            {
                if (ActualItem.gameObject.activeSelf)
                    ActualItem.gameObject.SetActive(false);
            }

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Show(float DeltaTime)
        {
            if (!ActualItem.gameObject.activeSelf)
                ActualItem.gameObject.SetActive(true);

            if (ActualItem.alpha < 1)
            {
                ActualItem.alpha += DeltaTime;
                var d = 1 - ActualItem.alpha;
                d *= -400;
                ActualItem.transform.localPosition = new Vector3(ActualItem.transform.localPosition.x, ActualItem.transform.localPosition.y, d);
            }
        }
    }
}
