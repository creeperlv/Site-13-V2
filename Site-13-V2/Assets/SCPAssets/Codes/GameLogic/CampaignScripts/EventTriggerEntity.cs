using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Site13Kernel.GameLogic.CampaignScripts
{
    public class EventTriggerEntity : MonoBehaviour
    {
        public DamagableEntity de;
        public string TargetSymbol;
        void Start()
        {
            if (de == null) de = GetComponentInChildren<DamagableEntity>();
            de.OnDie = () =>
            {
                FixedDirector.CurrentDirector.SetSymbol(TargetSymbol);
                return false;
            };
        }

    }

}