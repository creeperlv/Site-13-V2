using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic;
using Site13Kernel.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UEFI
{
    public class S13KSUtility : MonoBehaviour
    {
        public Text StartUpParameter;
        [Header("Menus")]
        public CanvasGroup MainMenu;
        public List<KVPair<string,GameObject>> MenuList=new List<KVPair<string,GameObject>>(); 
        [Header("Main Menu Buttons")]
        public UIButton GameSettingsButton;
        public UIButton ExitButton;
        [Header("Game Settings")]
        public GameObject GameSettingsGroup;
        public UIButton ClearAllSettings;
        public GameObject ClearDialog;
        public UIButton ClearDialog_Yes;
        [Header("Exit Function")]
        public GameObject ExitDialog;
        public UIButton EndProg;
        public UIButton ToSplashScreen;
        public int SplashSceneID;
        public UIButton CancelExit;
        void Start()
        {
            SelectFirstButton();
            StartUpParameter.text = Environment.CommandLine;
            ClearDialog_Yes.OnClick = () => { 
                foreach (var item in Directory.EnumerateFiles(GameEnv.DataPath))
                {
                    File.Delete(item);
                }
                foreach (var item in Directory.EnumerateDirectories(GameEnv.DataPath))
                {
                    Directory.Delete(item, true);
                }
                ClearDialog.SetActive(false);
                EnableMainMenu();
                ClearAllSettings.Select();
            };
            ExitButton.OnClick = () =>
            {
                DisableMainMenu();
                ExitDialog.SetActive(true);
                ToSplashScreen.Select();
            };
            ToSplashScreen.OnClick = () => {
                SceneLoader.Instance.LoadScene(SplashSceneID, true, false, false);
            };
            EndProg.OnClick = () => { Application.Quit(); };
            CancelExit.OnClick = () => { ExitDialog.SetActive(false);
                EnableMainMenu();
                SelectFirstButton(); };
        }
        public void ShowMenu(string Name)
        {
            foreach (var item in MenuList)
            {
                if (item.Key.ToUpper() == Name.ToUpper())
                {
                    item.Value.gameObject.SetActive(true);
                }else item.Value.gameObject.SetActive(false);
            }
        }
        public void HideMainMenu()
        {
            MainMenu.gameObject.SetActive(false);
        }
        public void DisableMainMenu()
        {
            MainMenu.interactable = false;
        }
        public void EnableMainMenu()
        {
            MainMenu.gameObject.SetActive(true);
            MainMenu.interactable = true;
            SelectFirstButton();
        }
        void SelectFirstButton()
        {
            GameSettingsButton.Select();
        }
    }

}