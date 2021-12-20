using Site13Kernel.Core;
using Site13Kernel.GameLogic;
using Site13Kernel.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel
{
    public class WinSceneMenu : MonoBehaviour
    {
        public GameObject NextLevelGroup;
        public GameObject WaitForDLCGroup;
        public Text NextLevelName;
        public UIButton NextLevel;
        public UIButton MainMenu;
        void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            MainMenu.OnClick = () => {
                GameRuntime.CurrentGlobals.CurrentMission = null;
                SceneLoader.Instance.ShowScene(3);
                SceneLoader.Instance.LoadScene(GameRuntime.CurrentGlobals.MainMenuSceneID, true, false, false);
            };
            var CM=GameRuntime.CurrentGlobals.CurrentMission;
            if (CM == null)
            {
                NextLevelGroup.SetActive(false);
                WaitForDLCGroup.SetActive(true);
                return;
            }
            var INDEX = GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections[0].MissionDefinitions.IndexOf(CM);
            if(INDEX>= GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections[0].MissionDefinitions.Count - 1)
            {
                NextLevelGroup.SetActive(false);
                WaitForDLCGroup.SetActive(true);
            }
            else
            {
                NextLevelGroup.SetActive(false);
                NextLevelGroup.SetActive(true);
                var NM = GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections[0].MissionDefinitions[INDEX + 1];
                NextLevelName.text = NM.DispFallback;
                NextLevel.OnClick = () => {
                    GameRuntime.CurrentGlobals.CurrentMission = NM;
                    SceneLoader.Instance.LoadScene(GameRuntime.CurrentGlobals.Scene_LevelLoader, true, false, false);
                };
            }
        }

    }
}
