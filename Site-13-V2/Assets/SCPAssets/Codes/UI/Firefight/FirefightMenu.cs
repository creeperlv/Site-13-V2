using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic;
using Site13Kernel.GameLogic.Firefight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.Firefight
{
    public class FirefightMenu : MonoBehaviour
    {
        public UIButton BackToMainmenu;
        public UIButton StartMission;
        public ComboBox GameMode;
        public ComboBox TimeMode;
        public Toggle AllowWeaponeSpawn;
        public GameObject MapButtonPrimitive;
        public GameObject TimeHeader;
        public FirefightMapButtonGroup BaseMapHolder;
        public Text MapName;
        public Text MapDesc;
        public Transform BasePresenter;
        public List<FirefightMapDefinition> BaseMaps = new List<FirefightMapDefinition>();
        public List<KVPair<string, Sprite>> BaseMapCoverMapping = new List<KVPair<string, Sprite>>();
        void Start()
        {
            if (SceneLoader.Instance != null)
                SceneLoader.Instance.HideScene(4);
            BackToMainmenu.OnClick = () =>
            {
                SceneLoader.Instance.EndLevel();
            };
            GameMode.onValueChanged.AddListener((v) =>
            {
                switch (v)
                {
                    case 0:
                        {
                            TimeHeader.SetActive(true);
                            TimeMode.gameObject.SetActive(true);
                        }
                        break;
                    case 1:
                        {
                            TimeHeader.SetActive(false);
                            TimeMode.gameObject.SetActive(false);
                        }
                        break;
                    default:
                        break;
                }
            });
            BaseMapHolder = new FirefightMapButtonGroup();
            foreach (var item in BaseMaps)
            {
                var btn = Instantiate(MapButtonPrimitive, BasePresenter).GetComponent<FirefightMapButton>();
                btn.CampaignParent = BaseMapHolder;
                btn.menu = this;
                btn.SetMap(item);
                btn.OnClick = () =>
                {
                    MapName.text = item.DisplayName;
                    MapDesc.text = item.Description;
                };
                BaseMapHolder.Children.Add(btn);
            }
            StartMission.OnClick = () =>
            {
                if (BaseMapHolder.Selected != null)
                {
                    FirefightLocals firefightLocals = new FirefightLocals();
                    var mission = BaseMapHolder.Selected.AssociatedMapDefinition;
                    var script = GenerateScript(BaseMapHolder.Selected.AssociatedMapDefinition.SceneID);
                    GameRuntime.CurrentGlobals.OneTimeScript = script;
                    FirefightMode mode = FirefightMode.LimitedTime;
                    if (GameMode.value == 1)
                    {
                        mode = FirefightMode.UnlimitedFirefight;
                    }
                    float Time = 5;
                    switch (TimeMode.value)
                    {
                        case 0:
                            Time = 0;
                            break;

                        case 1:
                            Time = 5;
                            break;
                        case 2:
                            Time = 10;
                            break;
                        case 3:
                            Time = 15;
                            break;
                        default:
                            break;
                    }
                    firefightLocals.CurrentDef = new FirefightDefinition() { GameMode = mode, AllowWeaponSpawn = AllowWeaponeSpawn.isOn, TimeLength = Time * 60 };
                    foreach (var item in BaseMapCoverMapping)
                    {
                        if (item.Key == mission.Image)
                        {
                            firefightLocals.Cover = item.Value;
                        }
                    }
                    firefightLocals.Title = mission.DisplayName;
                    firefightLocals.Desc = mission.Description;
                    FirefightLocals.Instance = firefightLocals;
                    SceneLoader.Instance.LoadScene("FIREFIGHTLOADER", true, true, false);
                    SceneLoader.Instance.Unload("FIREFIGHTUI");
                    SceneLoader.Instance.Unload(GameRuntime.CurrentGlobals.MainMenuSceneID);
                }
            };
        }
        public static string GenerateScript(int SceneID)
        {
            return @$"cls
echo FIREFIGHT!!!
visible 4 false
jump {SceneID} true true false
WAIT_FOR_LOAD
WAIT 1
active {SceneID}
unload FIREFIGHTLOADER
ffd
";
        }
    }
}
