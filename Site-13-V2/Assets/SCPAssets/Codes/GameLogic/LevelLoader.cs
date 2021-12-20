using Site13Kernel.Core;
using Site13Kernel.Diagnostics;
using Site13Kernel.Diagnostics.Errors;
using Site13Kernel.Diagnostics.Warns;
using Site13Kernel.Utilities;
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
            var Target = GameRuntime.CurrentGlobals.CurrentMission;
            if (Target == null)
            {
                Debugger.CurrentDebugger.LogError("Mission is null");
                Debugger.CurrentDebugger.LogWarning(new FallBackToMainMenuWarn());
            }
            else
            {
                CampaignCover.sprite=GameRuntime.CurrentGlobals.CurrentGameDef.Sprites[Target.ImageName].LoadedSprite;
                CampaignTitle.text = Target.DispFallback;
                CampaignDescription.text = Target.DescFallback;
                SceneLoader.Instance.LoadScene(SceneUtility.LookUp("LevelBase"), true, true, true);
            }
        }

    }
}
