using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Core.Interactives;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace Site13Kernel.GameLogic.CampaignScripts
{
    public class SimpleTrigger : ControlledBehavior, ITriggerable
    {
        List<Action> __callback = new List<Action>();

        public List<Action> Callback
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => __callback;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => __callback = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddCallback(Action Callback)
        {
            __callback.Add(Callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnTriggerEnter(Collider other)
        {
            var __FPS = other.gameObject.GetComponentInChildren<FPSController>();
            if (__FPS != null)
                if (__callback != null)
                {
                    foreach (var item in __callback)
                    {
                        item();
                    }
                }
        }
    }

}