using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.FPS;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.Combat
{
    public class GrenadeHUD : ControlledBehavior
    {
        public GameObject SelectionBorder;
        public Image IconImg;
        public Text NumberDisp;
        public int MonitoringPosition;
        public int LastGrenadeID = -1;
        public BagHolder holder;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (holder != null)
            {
                if (MonitoringPosition == 0)
                {
                    ApplyGrenade(holder.Grenade0);
                }
                else
                if (MonitoringPosition == 1)
                {
                    ApplyGrenade(holder.Grenade1);
                }

            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyGrenade(ProcessedGrenade GEN)
        {

            if (GEN.GrenadeHashCode != -1)
            {
                if (GEN.GrenadeHashCode != LastGrenadeID)
                {
                    LastGrenadeID = GEN.GrenadeHashCode;
                    this.gameObject.SetActive(true);
                    if (GrenadePool.CurrentPool.GrenadeItemMap.ContainsKey(LastGrenadeID))
                    {
                        IconImg.sprite = GrenadePool.CurrentPool.GrenadeItemMap[LastGrenadeID].Icon;
                    }
                }
            }
            else
            {
                if (LastGrenadeID != -1)
                {
                    LastGrenadeID = -1;
                    this.gameObject.SetActive(false);
                }
            }
            if (holder.CurrentGrenade == MonitoringPosition)
            {
                if (!SelectionBorder.activeSelf) SelectionBorder.SetActive(true);
            }else
                if (SelectionBorder.activeSelf) SelectionBorder.SetActive(false);
            NumberDisp.text = GEN.RemainingCount.ToString();
        }
    }
}
