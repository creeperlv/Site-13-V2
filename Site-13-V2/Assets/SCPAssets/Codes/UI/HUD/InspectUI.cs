using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.UI.HUD
{
    public class InspectUI : MonoBehaviour
    {
        public static InspectUI Instance;
        public InspectUIMode WorkMode;
        void Start()
        {
            Instance = this;
        }
        void ScoreBroad(float DeltaTime)
        {

        }
        void CampaignUI(float DeltaTime)
        {

        }
        void Show()
        {

        }
        void Hide()
        {

        }
        void Update()
        {
            var dt = Time.unscaledDeltaTime;
            switch (WorkMode)
            {
                case InspectUIMode.Scorebroad:
                    ScoreBroad(dt);
                    break;
                case InspectUIMode.CampaignUI:
                    CampaignUI(dt);
                    break;
                default:
                    break;
            }
        }
    }
    public enum InspectUIMode
    {
        Scorebroad, CampaignUI
    }
}
