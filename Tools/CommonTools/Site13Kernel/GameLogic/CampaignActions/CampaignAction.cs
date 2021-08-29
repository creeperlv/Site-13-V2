using Site13Kernel.Data;
using Site13Kernel.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.GameLogic.CampaignActions
{
    [Serializable]
    public class CampaignAction
    {
    }
    [Catalog("Scene")]
    [Description("Enables all objects under target scene.")]
    [Serializable]
    public class EnableSceneByName : CampaignAction
    {
        public string Name;
    }
    [Catalog("Scene")]
    [Description("Sets a scene as Active scene by name.")]
    [Serializable]
    public class SetActiveSceneByName : CampaignAction
    {
        public string Name;
    }
    [Catalog("Game Logic")]
    [Description("Wait for last scene is marked `Done`")]
    [Serializable]
    public class WaitUntilLastSceneDone : CampaignAction
    {
    }
    [Catalog("Game Logic")]
    [Description("Wait for player enters an AABB area.")]
    [Serializable]
    public class WaitForEnterAABB : CampaignAction
    {
        public float AX;
        public float AY;
        public float AZ;
        public float BX;
        public float BY;
        public float BZ;
    }
    [Catalog("Game Logic")]
    [Description("Wait for player health reachs a certain level.\r\nNegative: Smaller than value.\r\nPositive: Greater than value.")]
    [Serializable]
    public class WaitUntilHealth : CampaignAction
    {
        /// <summary>
        /// Negative: Smaller than value. Positive: Greater than value.
        /// </summary>
        public float Threshold;
    }
    [Catalog("Data")]
    [Description("Force sets a check point.")]
    [Serializable]
    public class SetCheckpoint : CampaignAction
    {

    }
    [Catalog("Game Logic")]
    [Description("Hides player.")]
    [Serializable]
    public class HidePlayer : CampaignAction
    {
    }
    [Catalog("Game Logic")]
    [Description("Shows player.")]
    [Serializable]
    public class ShowPlayer : CampaignAction
    {
    }
    [Catalog("Story")]
    [Description("Shows a subtitle (Main subtitle only).")]
    [Serializable]

    public class ShowSubtitle : CampaignAction
    {
        public string ID;
        public string Fallback;
        public float Duration;
    }
    [Catalog("Game Logic")]
    [Description("The end mark of a campaign mission.")]
    [Serializable]
    public class EndCampaign : CampaignAction
    {

    }
}
