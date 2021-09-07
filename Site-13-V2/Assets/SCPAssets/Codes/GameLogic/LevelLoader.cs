using Site13Kernel.Core;
using Site13Kernel.Diagnostics;
using Site13Kernel.Diagnostics.Errors;
using Site13Kernel.Diagnostics.Warns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.GameLogic
{
    public class LevelLoader : MonoBehaviour
    {
        public Image CampaignCover;
        public Text CampaignTitle;
        public Text CampaignDescription;
        void Start()
        {
            var Target = GameRuntime.CurrentGlobals.NextCampaign;
            if (GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections.Count > GameRuntime.CurrentGlobals.NextCampaign)
            {
                var M = GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections[0].MissionDefinitions[Target];
            }
            else
            {
                Debugger.CurrentDebugger.Log(new UndefinedCampaignID(Target));
                Debugger.CurrentDebugger.Log(new FallBackToMainMenuWarn());
            }
        }

    }
}
