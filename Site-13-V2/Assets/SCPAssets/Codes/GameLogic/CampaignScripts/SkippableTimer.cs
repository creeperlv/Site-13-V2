using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.GameLogic.RuntimeScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.GameLogic.CampaignScripts
{
    public class SkippableTimer : MonoBehaviour
    {
        public bool IsSet;
        public string InputName;
        public float TargetTime;
        public string Symbol;
        public bool TargetValue = true;
        public GameObject Prompt;
        public Image PromptProgress;
        public float SkipTimer=300;
        public float SkipTimerD=0;
        float CT;
        // Update is called once per frame
        void Update()
        {
            if (IsSet)
                return;
            CT += Time.deltaTime;
            if (InputProcessor.GetInput(InputName))
            {
                if (!Prompt.activeSelf)
                    Prompt.SetActive(true);
            }
            else
            {
                SkipTimerD = 0;
                if (Prompt.activeSelf)
                    Prompt.SetActive(false);
            }
            if (CT > TargetTime)
            {
                LevelRuntimeRegistry.Set(Symbol, TargetValue);
                IsSet = true;
                enabled = false;
            }
        }
    }
}
