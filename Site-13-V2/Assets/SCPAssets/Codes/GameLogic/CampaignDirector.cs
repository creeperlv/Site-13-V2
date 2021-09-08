using Site13Kernel.Core;
using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Site13Kernel.GameLogic
{
    public class CampaignDirector : ControlledBehavior
    {
        SerialCampaignScript script;
        public override void Init()
        {
            script = GameRuntime.CurrentLocals.CurrentScipt;
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            var d=script.CurrentData();
            //script.NextData();
        }
        public override void FixedRefresh(float DeltaTime, float UnscaledDeltaTime)
        {
        }
    }
}
