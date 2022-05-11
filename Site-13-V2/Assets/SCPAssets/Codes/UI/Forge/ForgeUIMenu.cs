using Newtonsoft.Json;
using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic;
using Site13Kernel.SceneBuild;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.Forge
{
    public class ForgeUIMenu : MonoBehaviour
    {
        public Transform MapPresenter;
        public Transform MapPresenterHolder;
        public Transform BasePresenter;
        public Transform BrowsePage;
        public Transform CreatePage;
        public UIButton BrowseBtn;
        public UIButton LoadMap;
        public UIButton EditMap;
        public UIButton Back;
        public Text MapName;
        public Text MapDesc;
        public Transform NoMapHint;
        public Text ErrorHintHumanRead;
        public Text ErrorHintTetvatRead;
        public GameObject ForgeMapPrefab;
        [Header("Map Creation")]
        public TextBox NameBox;
        public TextBox DescBox;
        public UIButton CreateBtn;
        public UIButton CreateWorldBtn;
        public List<ForgeMap> BaseMaps = new List<ForgeMap>();
        public List<KVPair<string, Sprite>> BaseMapCoverMapping = new List<KVPair<string, Sprite>>();
        ForgeMapButtonGroup MapHolder;
        ForgeMapButtonGroup BaseMapHolder;
        void Start()
        {
            BrowseBtn.OnClick = () =>
            {
                BrowsePage.gameObject.SetActive(true);
                CreatePage.gameObject.SetActive(false);
            };
            CreateBtn.OnClick = () =>
            {
                BrowsePage.gameObject.SetActive(false);
                CreatePage.gameObject.SetActive(true);
            };
            Back.OnClick = () =>
            {
                SceneLoader.Instance.EndLevel();
            };
            if (GameEnv.DataPath == null)
            {
                GameEnv.DataPath = Application.persistentDataPath;
            }
            MapHolder = new ForgeMapButtonGroup();
            BaseMapHolder = new ForgeMapButtonGroup();
            var ForgeMapFolder = Path.Combine(GameEnv.DataPath, "Forge");
            try
            {
                if (!Directory.Exists(ForgeMapFolder)) Directory.CreateDirectory(ForgeMapFolder);
                var files = Directory.EnumerateFiles(ForgeMapFolder).ToList();
                if (files.Count == 0)
                {
                    ShowError("No Map");
                }
                else
                {
                    NoMapHint.gameObject.SetActive(false);
                    MapPresenterHolder.gameObject.SetActive(true);
                    foreach (var _file in files)
                    {
                        if (_file.EndsWith(".json"))
                        {
                            var ForgeMap = JsonConvert.DeserializeObject<ForgeMap>(File.ReadAllText(_file), SceneBuilder.settings);
                            var btn = Instantiate(ForgeMapPrefab, MapPresenter).GetComponent<ForgeMapButton>();
                            btn.CampaignParent = MapHolder;
                            btn.menu = this;
                            btn.SetMap(ForgeMap);
                            btn.OnClick = () =>
                            {
                                MapName.text = ForgeMap.DisplayName;
                                MapDesc.text = ForgeMap.Description;
                            };
                            MapHolder.Children.Add(btn);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                ShowError("Cannot access forge map folder");
            }
            foreach (var item in BaseMaps)
            {
                var btn = Instantiate(ForgeMapPrefab, BasePresenter).GetComponent<ForgeMapButton>();
                btn.CampaignParent = BaseMapHolder;
                btn.menu = this;
                btn.SetMap(item);
                BaseMapHolder.Children.Add(btn);
            }
            EditMap.OnClick = () =>
            {
                EnterMap(ForgeMapFolder);
            };
            LoadMap.OnClick = () =>
            {
                EnterMap(ForgeMapFolder,true);
            };
            CreateWorldBtn.OnClick = () =>
            {
                GameRuntime.CurrentGlobals.MainUIBGM.Pause();
                var MapDef = BaseMapHolder.Selected.AssociatedMapDefinition.Duplicate();
                Guid guid = Guid.NewGuid();
                MapDef.MapID = guid.ToString();
                MapDef.DisplayName = NameBox.text;
                MapDef.Description = DescBox.text;
                MapDef.SceneDescriptionFile = MapDef.MapID + ".fsd";//forge scene description.
                var _map = Path.Combine(ForgeMapFolder, guid.ToString() + ".json");
                var __map = Path.Combine(ForgeMapFolder, guid.ToString() + ".fsd");
                if (File.Exists(_map)) File.Delete(_map);
                if (File.Exists(__map)) File.Delete(__map);
                File.WriteAllText(_map, JsonConvert.SerializeObject(MapDef));
                ForgeLocals l = new ForgeLocals();
                l.BaseMap = MapDef.BaseMapID;
                l.SceneDescriptionFile = new FileInfo(__map);
                ForgeLocals.Config(l);
                SceneLoader.Instance.HideScene(4);
                SceneLoader.Instance.LoadScene(10, true, false, false);
            };
        }

        private void EnterMap(string ForgeMapFolder, bool isReadOnly = false)
        {
            GameRuntime.CurrentGlobals.MainUIBGM.Pause();
            var __SelectedBtn = MapHolder.Selected;
            var __def = __SelectedBtn.AssociatedMapDefinition.Duplicate();
            var __desc_file = Path.Combine(ForgeMapFolder, __def.SceneDescriptionFile);
            ForgeLocals l = new ForgeLocals();
            l.BaseMap = __def.BaseMapID;
            l.isReadOnly = isReadOnly;
            l.SceneDescriptionFile = new FileInfo(__desc_file);
            ForgeLocals.Config(l);
            SceneLoader.Instance.HideScene(4);
            SceneLoader.Instance.LoadScene(10, true, false, false);
        }

        void ShowError(string hint)
        {
            NoMapHint.gameObject.SetActive(true);
            MapPresenterHolder.gameObject.SetActive(false);
            ErrorHintHumanRead.text = hint;
            ErrorHintTetvatRead.text = hint;
        }

    }

}