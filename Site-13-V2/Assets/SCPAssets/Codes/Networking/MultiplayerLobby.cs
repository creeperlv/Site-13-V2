using Site13Kernel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Networking
{
    public class MultiplayerLobby : MonoBehaviour
    {
        public MultiplayerConfiguration configuration;
        public GameObject ControlPanel;
        void Start()
        {
            if (Application.isBatchMode) ControlPanel.SetActive(false);
            else InitUI();
        }
        void InitUI()
        {

        }
        void Update()
        {
            if (!Application.isBatchMode)
            {
                UpdateUI();
            }
        }
        void UpdateUI()
        {

        }
    }
    [Serializable]
    public class MultiplayerConfiguration {
        public int MaxPlayerPerTeam;
    }

}
