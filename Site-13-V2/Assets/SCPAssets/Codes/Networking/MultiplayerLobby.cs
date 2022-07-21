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

        public GenericInteractivable StartButton;
        public GenericInteractivable CloseButton;
        public GenericInteractivable UpdateButton;
        void Start()
        {
            if (Application.isBatchMode) { 
                ControlPanel.SetActive(false); 
                CLILoop();
            }
            else InitUI();
        }
        void InitUI()
        {
            PlayerCount.text = configuration.MaxPlayerPerTeam+"";
            CloseButton.OnClick += () => {
                Application.Quit();
            };
        }
        void CLILoop()
        {
            Task.Run(() => {
                bool WillExit = false;
                while (!WillExit)
                {
                    var L=Console.ReadLine();
                    if (L.ToUpper() == "EXIT")
                    {
                        Console.WriteLine("Goodbye!");
                        Environment.Exit(0);
                    }
                    var Parameters=CLUNL.Utilities.CommandLineTool.Analyze(L);
                    
                }
            });
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
