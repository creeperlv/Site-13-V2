using Site13Kernel.Core;
using Site13Kernel.UI.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.Networking
{
    public class MultiplayerLobby : MonoBehaviour
    {
        public MultiplayerConfiguration configuration;
        public GameObject ControlPanel;
        public InputField PlayerCount;
        public InputField RoomTitle;
        public InputField RoomDescription;
        public HorizontalLayoutGroup MenuBar;
        public GenericInteractivable StartButton;
        public GenericInteractivable CloseButton;
        public GenericInteractivable UpdateButton;
        void Start()
        {
            if (Application.isBatchMode)
            {
                ControlPanel.SetActive(false);
                LobbyService();
            }
            else InitUI();
        }
        void InitUI()
        {
            UnityEngine.Screen.SetResolution(800, 600, false);
            PlayerCount.text = configuration.MaxPlayerPerTeam + "";
            CloseButton.OnClick += () =>
            {
                Application.Quit();
            };
            StartCoroutine(ResetMenuBar());
        }
        IEnumerator ResetMenuBar()
        {
            yield return null;
            MenuBar.gameObject.SetActive(false);
            yield return null;
            MenuBar.gameObject.SetActive(true);
        }
        void LobbyService()
        {
            Diagnostics.Debug.Log("Starting Lobby Service");
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
    public class MultiplayerConfiguration
    {
        public int MaxPlayerPerTeam;
    }

}
