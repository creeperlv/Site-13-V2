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
        public List<GameObject> NextLevelGroup;
        public List<GameObject> WaitForDLCGroup;
        public Text NextLevelName;
        public UIButton NextLevel;
        public UIButton MainMenu;
        void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            MainMenu.OnClick = () => {
                GameRuntime.CurrentGlobals.CurrentMission = null;
                SceneLoader.Instance.EndLevel();
                //SceneLoader.Instance.ShowScene(4);
                //SceneLoader.Instance.Unload("LEVELBASE");
                //SceneLoader.Instance.LoadScene(GameRuntime.CurrentGlobals.MainMenuSceneID, true, false, false);
            };
            var CM=GameRuntime.CurrentGlobals.CurrentMission;
            if (CM == null)
            {
                foreach (var item in NextLevelGroup)
                {
                    item.SetActive(false);
                }
                foreach (var item in WaitForDLCGroup)
                {
                    item.SetActive(true);
                }
                NextLevel.gameObject.SetActive(false);
                return;
            }
            var INDEX = GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections[0].MissionDefinitions.IndexOf(CM);
            if(INDEX>= GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections[0].MissionDefinitions.Count - 1)
            {
                foreach (var item in NextLevelGroup)
                {
                    item.SetActive(false);
                }
                foreach (var item in WaitForDLCGroup)
                {
                    item.SetActive(true);
                }
                NextLevel.gameObject.SetActive(false);
            }
            else
            {
                foreach (var item in NextLevelGroup)
                {
                    item.SetActive(true);
                }
                foreach (var item in WaitForDLCGroup)
                {
                    item.SetActive(false);
                }
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
