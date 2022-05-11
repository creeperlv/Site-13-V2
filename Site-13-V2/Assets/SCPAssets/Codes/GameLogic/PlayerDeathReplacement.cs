using Site13Kernel.Core.Controllers;
using Site13Kernel.GameLogic.CampaignScripts;
using Site13Kernel.UI;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic
{
    public class PlayerDeathReplacement : MonoBehaviour
    {
        public UIButton Revive;
        public UIButton MainMenu;
        void Start()
        {
            Cursor.lockState= CursorLockMode.None;
            Cursor.visible = true;
            MainMenu.OnClick = () => {
                SceneLoader.Instance.EndLevel();
            };
            Revive.OnClick = () => {
                var PLAYER = GlobalBioController.CurrentGlobalBioController.Spawn(FixedDirector.CurrentDirector.DefaultPlayer.Key, Vector3.zero, Vector3.zero);
                if (FixedDirector.CurrentDirector.RespawnPoints.Count > 0)
                {
                    var t=Maths.ObtainOne(FixedDirector.CurrentDirector.RespawnPoints);
                    PLAYER.transform.GetChild(1).position = t.position;
                    PLAYER.transform.GetChild(1).rotation = t.rotation;
                }
                else
                {
                    PLAYER.transform.GetChild(1).position = transform.position;
                    PLAYER.transform.GetChild(1).rotation = transform.rotation;
                }
                var FPSC = PLAYER.GetComponentInChildren<FPSController>();

                FixedDirector.CurrentDirector.LevelController.RegisterRefresh(FPSC);
                FPSC.Parent = FixedDirector.CurrentDirector.LevelController;
                FPSC.Init();
                FPSC.GiveWeapon(FixedDirector.CurrentDirector.DefaultWeapon);
                Destroy(gameObject);
            };
        }

    }
}
