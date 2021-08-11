using Site13GameData.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13GameData.GameLogic.CampaignActions
{
    [Serializable]
    public class CampaignAction
    {
    }
    [Serializable]
    public class LoadSceneByName : CampaignAction
    {
        public string Name;
    }
    [Serializable]
    public class WaitUntilLastSceneDone : CampaignAction
    {

    }
    [Serializable]
    public class WaitForEnterAABB : CampaignAction
    {
        public float3 A;
        public float3 B;
    }
    [Serializable]
    public class WaitUntilHealth : CampaignAction
    {
        /// <summary>
        /// Negative: Smaller than value. Positive: Greater than value.
        /// </summary>
        public float Threshold;
    }
    [Serializable]
    public class SetCheckpoint : CampaignAction
    {

    }
    [Serializable]
    public class HidePlayer : CampaignAction
    {
    }
    [Serializable]
    public class ShowPlayer : CampaignAction
    {
    }
}
