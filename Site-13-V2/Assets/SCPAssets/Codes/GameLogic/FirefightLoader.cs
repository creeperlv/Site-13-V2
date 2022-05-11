using Site13Kernel.Core;
using Site13Kernel.Diagnostics;
using Site13Kernel.Diagnostics.Errors;
using Site13Kernel.Diagnostics.Warns;
using Site13Kernel.GameLogic.Firefight;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.GameLogic
{
    public class FirefightLoader : MonoBehaviour
    {
        public Image CampaignCover;
        public Text CampaignTitle;
        public Text CampaignDescription;
        void Start()
        {
            if (Firefight.FirefightLocals.Instance == null)
            {
                Debugger.CurrentDebugger.LogError("Firefight Local is null, going to Aszod...");
            }
            else
            {
                CampaignCover.sprite=FirefightLocals.Instance.Cover;
                CampaignTitle.text = FirefightLocals.Instance.Title;
                CampaignDescription.text = FirefightLocals.Instance.Desc;
                SceneLoader.Instance.LoadScene(SceneUtility.LookUp("LevelBase"), true, true, true);
            }
        }

    }
}
